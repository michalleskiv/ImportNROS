using System;
using System.Threading.Tasks;
using ImportBL.Exceptions;
using ImportBL.Interfaces;
using ImportBL.Models;

namespace ImportBL
{
    /// <summary>
    /// Main class, that manages all processes
    /// </summary>
    public class MainWorker : IMainWorker
    {
        private readonly IConfiguration _configuration;
        private readonly IDataReceiver _dataReceiver;
        private readonly IFileReader _fileReader;
        private readonly IDataPair _dataPair;
        private readonly IDataSender _dataSender;
        private readonly ILogger _logger;
        private readonly ISessionIdGenerator _generator;
        private readonly IContactUpdater _updater;

        public double Progress { get; set; }
        public bool IsFree { get; set; } = true;

        /// <summary>
        /// Call if state was changed
        /// </summary>
        public event EventHandler StateChanged;

        public MainWorker(IDataReceiver dataReceiver, IFileReader fileReader, IDataPair dataPair, IDataSender dataSender, 
            ISessionIdGenerator generator, IContactUpdater updater, ILogger logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _dataReceiver = dataReceiver;
            _fileReader = fileReader;
            _dataPair = dataPair;
            _dataSender = dataSender;
            _generator = generator;
            _updater = updater;
        }
        
        public async Task Run(string filePath)
        {
            IsFree = false;

            _logger.StateChanged += StateChanged;
            double toAdd = 100.0 / 8.0;

            try
            {
                Progress = 0;
                _logger.LogInfo($"Session ID is {_generator.Id}");

                // Load data
                _logger.LogInfo("Getting contacts from Tabidoo...");
                var tabidooContacts = await _dataReceiver.GetTable<Contact>(_configuration.ContactSchemaId);
                Progress += toAdd;
                
                _logger.LogInfo("Getting subjects from Tabidoo...");
                var tabidooSubjects = await _dataReceiver.GetTable<Subject>(_configuration.SubjectSchemaId);
                Progress += toAdd;

                _logger.LogInfo("Getting gifts from Tabidoo...");
                var tabidooGifts = await _dataReceiver.GetTable<Gift>(_configuration.GiftSchemaId);
                Progress += toAdd;

                // Read gifts from Excel
                _logger.LogInfo("Reading gifts from Tabidoo...");
                var excelGifts = _fileReader.ReadGifts(filePath);
                Progress += toAdd;

                _generator.MarkGifts(excelGifts);

                // Pairing data
                _logger.LogInfo("Pairing data...");
                var contactsToInsert = _dataPair.ConnectData(excelGifts, tabidooContacts, tabidooSubjects);
                _dataPair.ConnectData(tabidooGifts, tabidooContacts, tabidooSubjects);
                Progress += toAdd;

                // Send contacts
                _logger.LogInfo("Sending contacts to Tabidoo...");
                await _dataSender.SendItems(_configuration.ContactSchemaId, contactsToInsert);
                Progress += toAdd;

                // send gifts
                _logger.LogInfo("Sending gifts to Tabidoo...");
                await _dataSender.SendItems(_configuration.GiftSchemaId, excelGifts);
                Progress += toAdd;

                //Fix it
                tabidooGifts.AddRange(excelGifts);
                _updater.UpdateTags(tabidooContacts, tabidooGifts);

                // Update contacts in Tabidoo
                _logger.LogInfo("Updating contacts in Tabidoo...");
                await _dataSender.UpdateContacts(_configuration.ContactSchemaId, tabidooContacts);
                Progress += toAdd;

                await _logger.LogToFile(_configuration.LogFilePath);
                Progress = 100;
                _logger.LogInfo("Done!");
            }
            catch (LocalException e)
            {
                _logger.LogException(e);
            }
            finally
            {
                IsFree = true;
            }
        }
        
        public string GetNewLogs()
        {
            return _logger.GetNewLogs();
        }
    }
}
