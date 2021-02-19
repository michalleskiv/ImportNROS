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
                var tabidooContacts = await _dataReceiver.GetTable<Contact>(_configuration.ContactSchemaId);
                Progress += toAdd;
                _logger.LogInfo("Contacts have been got from Tabidoo");
                var tabidooSubjects = await _dataReceiver.GetTable<Subject>(_configuration.SubjectSchemaId);
                Progress += toAdd;
                _logger.LogInfo("Subjects have been got from Tabidoo");
                var tabidooGifts = await _dataReceiver.GetTable<Gift>(_configuration.GiftSchemaId);
                Progress += toAdd;
                _logger.LogInfo("Gifts have been got from Tabidoo");

                // Read gifts from Excel
                var excelGifts = _fileReader.ReadGifts(filePath);
                Progress += toAdd;
                _logger.LogInfo("Gifts have been read from Excel document");

                _generator.MarkGifts(excelGifts);

                // Pairing data
                var contactsToInsert = _dataPair.ConnectData(excelGifts, tabidooContacts, tabidooSubjects);
                _dataPair.ConnectData(tabidooGifts, tabidooContacts, tabidooSubjects);
                Progress += toAdd;
                _logger.LogInfo("Data have been paired");

                // Send contacts
                await _dataSender.SendItems(_configuration.ContactSchemaId, contactsToInsert);
                Progress += toAdd;
                _logger.LogInfo("Contacts have been sent to Tabidoo");

                // send gifts
                await _dataSender.SendItems(_configuration.GiftSchemaId, excelGifts);
                Progress += toAdd;
                _logger.LogInfo("Gifts have been sent to Tabidoo");

                //Fix it
                tabidooGifts.AddRange(excelGifts);
                _updater.UpdateTags(tabidooContacts, tabidooGifts);

                // Update contacts in Tabidoo
                await _dataSender.UpdateContacts(_configuration.ContactSchemaId, tabidooContacts);
                Progress += toAdd;
                _logger.LogInfo("Contacts have been updated in Tabidoo");

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
