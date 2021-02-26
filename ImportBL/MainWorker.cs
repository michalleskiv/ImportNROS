using System;
using System.Collections.Generic;
using System.Linq;
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
                _logger.LogInfo("Expected order of columns:\n1. Datum daru;\n2. Aktivity;\n3. Email;\n4. Jmeno;\n5. Prijmeni;\n6. Castka;\n7. Cislo uctu;\n8. Kod banky;\n9. Na ucet;" +
                                "\n10. Variabilni symbol;\n11. Platebli metoda;" +
                                "\n12. Stav platby;\n13. Potvrzeni o daru;\n14. Poznamka;\n15. Specificky symbol;\n16. Zdroj daru;\n17. Ucel daru;\n18. Dar ocisteny o");

                // Load data
                _logger.LogInfo("Getting contacts from Tabidoo...");
                var tabidooContacts = await _dataReceiver.GetTable<Contact>(_configuration.ContactSchemaId);
                _logger.LogInfo($"{tabidooContacts.Count} contacts have been got from Tabidoo");
                Progress += toAdd;
                
                _logger.LogInfo("Getting subjects from Tabidoo...");
                var tabidooSubjects = await _dataReceiver.GetTable<Subject>(_configuration.SubjectSchemaId);
                _logger.LogInfo($"{tabidooSubjects.Count} subjects have been got from Tabidoo");
                Progress += toAdd;

                _logger.LogInfo("Getting gifts from Tabidoo...");
                var tabidooGifts = await _dataReceiver.GetTable<Gift>(_configuration.GiftSchemaId);
                _logger.LogInfo($"{tabidooGifts.Count} gifts have been got from Tabidoo");
                Progress += toAdd;

                // Read gifts from Excel
                _logger.LogInfo("Reading gifts from Excel...");
                var excelGifts = _fileReader.ReadGifts(filePath);
                _logger.LogInfo($"{excelGifts.Count} gifts have been read from Excel");
                Progress += toAdd;

                _generator.MarkGifts(excelGifts);

                // Pairing data
                _logger.LogInfo("Pairing data...");
                var contactsToInsert = _dataPair.ConnectDataExcel(excelGifts, tabidooContacts, tabidooSubjects);
                _dataPair.ConnectDataTabidoo(tabidooGifts, tabidooContacts);
                Progress += toAdd;

                // Send contacts
                _logger.LogInfo("Sending contacts to Tabidoo...");
                await _dataSender.SendItems(_configuration.ContactSchemaId, contactsToInsert);
                _logger.LogInfo($"{_logger.SuccessfullyItemsSent} contacts have been successfully sent to Tabidoo");
                _logger.LogInfo($"{_logger.ErroneousItemsSent} errors while sending contacts");
                Progress += toAdd;

                _logger.SuccessfullyItemsSent = 0;
                _logger.ErroneousItemsSent = 0;

                // send gifts
                _logger.LogInfo("Sending gifts to Tabidoo...");
                await _dataSender.SendItems(_configuration.GiftSchemaId, excelGifts);
                _logger.LogInfo($"{excelGifts.Count - _logger.ErroneousItemsSent} gifts have been successfully sent to Tabidoo");
                _logger.LogInfo($"{_logger.ErroneousItemsSent} errors while sending gifts");
                Progress += toAdd;

                var contactsToUpdate = excelGifts.Select(g => g.Kontakt).Where(k => k != null).ToList();

                MergeGifts(tabidooGifts, excelGifts);
                _updater.UpdateTags(contactsToUpdate, tabidooGifts);

                // Update contacts in Tabidoo
                _logger.LogInfo("Updating contacts in Tabidoo...");
                await _dataSender.UpdateContacts(_configuration.ContactSchemaId, contactsToUpdate);
                _logger.LogInfo($"{_logger.SuccessfullyContactsUpdated} contacts have been successfully updated in Tabidoo");
                _logger.LogInfo($"{_logger.ErroneousContactsUpdated} errors while updating contacts");
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

        private void MergeGifts(List<Gift> tabidooGifts, List<Gift> excelGifts)
        {
            foreach (var excelGift in excelGifts)
            {
                if (excelGift.Id != null)
                {
                    tabidooGifts.Add(excelGift);
                }
            }
        }
    }
}
