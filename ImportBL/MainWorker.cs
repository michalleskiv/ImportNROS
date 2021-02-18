using System;
using System.Threading.Tasks;
using ImportBL.Exceptions;
using ImportBL.Interfaces;
using ImportBL.Models;
using Microsoft.Extensions.Configuration;

namespace ImportBL
{
    public class MainWorker
    {
        private readonly IDataReceiver _dataReceiver;
        private readonly IFileReader _fileReader;
        private readonly IDataPair _dataPair;
        private readonly IDataSender _dataSender;
        private readonly ILogger _logger;
        private readonly ISessionIdGenerator _generator;

        public int Progress { get; set; }

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
        }

        public async Task Run(string filePath)
        {
            _logger.StateChanged += StateChanged;
            int toAdd = 100 / 7;

            try
            {
                Progress = 0;
                _logger.LogInfo($"Session ID is {_generator.Id}");

                var tabidooContacts = await _dataReceiver.GetTable<Contact>(Configuration.ContactSchemaId);
                Progress += toAdd;
                _logger.LogInfo("Contacts were got from Tabidoo");
                
                var tabidooSubjects = await _dataReceiver.GetTable<Subject>(Configuration.SubjectSchemaId);
                Progress += toAdd;
                _logger.LogInfo("Subjects were got from Tabidoo");
                
                var tabidooGifts = await _dataReceiver.GetTable<Gift>(Configuration.GiftSchemaId);
                Progress += toAdd;
                _logger.LogInfo("Gifts were got from Tabidoo");

                var excelGifts = _fileReader.ReadGifts(filePath);
                Progress += toAdd;
                _logger.LogInfo("Gifts were read from Excel document");

                _generator.MarkGifts(excelGifts);

                var contactsToInsert = _dataPair.ConnectData(excelGifts, tabidooContacts, tabidooSubjects);
                Progress += toAdd;
                _logger.LogInfo("Data were paired");
                
                await _dataSender.SendItems(Configuration.ContactSchemaId, contactsToInsert);
                Progress += toAdd;
                _logger.LogInfo("Contacts were sent to Tabidoo");

                await _dataSender.SendItems(Configuration.GiftSchemaId, excelGifts);
                Progress += toAdd;
                _logger.LogInfo("Gifts were sent to Tabidoo");

                await _logger.LogToFile(Configuration.LogFilePath);
                Progress = 100;
                _logger.LogInfo("Done!");
            }
            catch(LocalException e)
            {
                _logger.LogException(e);
            }
        }

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
