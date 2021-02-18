using System.Threading.Tasks;
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

        public MainWorker()
        {
            SetConfig();

            _logger = new Logger();

            _dataReceiver = new TabidooDataReceiver(Configuration.Url, Configuration.AppId, Configuration.Token);
            _fileReader = new FileReader();
            _dataPair = new DataPair();
            _dataSender = new TabidooDataSender(Configuration.Url, Configuration.AppId, Configuration.Token, _logger);
        }

        public async Task Run(string filePath)
        {
            var tabidooContacts = await _dataReceiver.GetTable<Contact>(Configuration.ContactSchemaId);
            var tabidooSubjects = await _dataReceiver.GetTable<Subject>(Configuration.SubjectSchemaId);
            var tabidooGifts = await _dataReceiver.GetTable<Gift>(Configuration.GiftSchemaId);

            var excelGifts = _fileReader.ReadGifts(filePath);

            var contactsToInsert = _dataPair.ConnectData(excelGifts, tabidooContacts, tabidooSubjects);

            await _dataSender.SendItems(Configuration.ContactSchemaId, contactsToInsert);
            await _dataSender.SendItems(Configuration.GiftSchemaId, excelGifts);
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
