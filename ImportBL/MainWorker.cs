using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ImportBL
{
    public class MainWorker
    {
        public string FilePath { get; set; }

        public async Task Run()
        {

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
            Configuration.FilePath = config["firePath"];
            Configuration.LogFilePath = config["logFilePath"] ?? "log.txt";
        }
    }
}
