using ImportBL.Interfaces;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using IConfiguration = ImportBL.Interfaces.IConfiguration;

namespace ImportBL
{
    public static class DiContainer
    {
        public static Container Container { get; set; } = new Container();

        static DiContainer()
        {
            Container.Register<IConfiguration, Configuration>(Lifestyle.Singleton);
            Container.Register<ILogger, Logger>();
            Container.Register<IDataReceiver, TabidooDataReceiver>();
            Container.Register<IDataPair, DataPair>();
            Container.Register<IDataSender, TabidooDataSender>();
            Container.Register<IContactUpdater, ContactUpdater>();
            Container.Register<IFileReader, FileReader>();
            Container.Register<ISessionIdGenerator, SessionIdGenerator>();
            Container.Register<IMainWorker, MainWorker>();

            Microsoft.Extensions.Configuration.IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("config.json", true, true)
                .Build();

            var configuration = Container.GetInstance<IConfiguration>();
            configuration.Url = config["url"];
            configuration.AppId = config["appId"];
            configuration.ContactSchemaId = config["contactSchemaId"];
            configuration.SubjectSchemaId = config["subjectSchemaId"];
            configuration.GiftSchemaId = config["giftSchemaId"];
            configuration.Token = config["token"];
            configuration.LogFilePath = config["logFilePath"];
        }
    }
}
