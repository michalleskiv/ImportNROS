using System;
using System.Threading.Tasks;
using ImportBL.Exceptions;
using ImportBL.Interfaces;
using ImportBL.Models;
using Microsoft.Extensions.Configuration;

namespace ImportBL
{
    /// <summary>
    /// Main class, that manages all processes
    /// </summary>
    public class MainWorker
    {
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

        public MainWorker()
        {
            SetConfig();

            _logger = new Logger();

            _dataReceiver = new TabidooDataReceiver(Configuration.Url, Configuration.AppId, Configuration.Token, _logger);
            _fileReader = new FileReader(_logger);
            _dataPair = new DataPair(_logger);
            _dataSender = new TabidooDataSender(Configuration.Url, Configuration.AppId, Configuration.Token, _logger);
            _generator = new SessionIdGenerator();
            _updater = new ContactUpdater();
        }

        /// <summary>
        /// Run processing
        /// </summary>
        /// <param name="filePath">Path to the Excel document</param>
        /// <returns></returns>
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
                var tabidooContacts = await _dataReceiver.GetTable<Contact>(Configuration.ContactSchemaId);
                Progress += toAdd;
                _logger.LogInfo("Contacts had got from Tabidoo");
                var tabidooSubjects = await _dataReceiver.GetTable<Subject>(Configuration.SubjectSchemaId);
                Progress += toAdd;
                _logger.LogInfo("Subjects had got from Tabidoo");
                var tabidooGifts = await _dataReceiver.GetTable<Gift>(Configuration.GiftSchemaId);
                Progress += toAdd;
                _logger.LogInfo("Gifts had got from Tabidoo");

                // Read gifts from Excel
                var excelGifts = _fileReader.ReadGifts(filePath);
                Progress += toAdd;
                _logger.LogInfo("Gifts had read from Excel document");

                _generator.MarkGifts(excelGifts);

                // Pairing data
                var contactsToInsert = _dataPair.ConnectData(excelGifts, tabidooContacts, tabidooSubjects);
                _dataPair.ConnectData(tabidooGifts, tabidooContacts, tabidooSubjects);
                Progress += toAdd;
                _logger.LogInfo("Data paired");

                // Send contacts
                await _dataSender.SendItems(Configuration.ContactSchemaId, contactsToInsert);
                Progress += toAdd;
                _logger.LogInfo("Contacts had sent to Tabidoo");

                // send gifts
                await _dataSender.SendItems(Configuration.GiftSchemaId, excelGifts);
                Progress += toAdd;
                _logger.LogInfo("Gifts had sent to Tabidoo");

                //Fix it
                tabidooGifts.AddRange(excelGifts);
                _updater.UpdateTags(tabidooContacts, tabidooGifts);

                // Update contacts in Tabidoo
                await _dataSender.UpdateContacts(Configuration.ContactSchemaId, tabidooContacts);
                Progress += toAdd;
                _logger.LogInfo("Contacts have been updated in Tabidoo");

                await _logger.LogToFile(Configuration.LogFilePath);
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

        /// <summary>
        /// Return new logs
        /// </summary>
        /// <returns>All new logs</returns>
        public string GetNewLogs()
        {
            return _logger.GetNewLogs();
        }

        private void SetConfig()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("config.json", true, true)
                .Build();

            Configuration.Url = config["url"];
            Configuration.AppId = config["appId"];
            Configuration.ContactSchemaId = config["contactSchemaId"];
            Configuration.SubjectSchemaId = config["subjectSchemaId"];
            Configuration.GiftSchemaId = config["giftSchemaId"];
            Configuration.Token = config["token"];
            Configuration.LogFilePath = config["logFilePath"] ?? "log.txt";
        }
    }
}
