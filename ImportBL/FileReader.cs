using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using ExcelDataReader;
using ImportBL.Exceptions;
using ImportBL.Interfaces;
using ImportBL.Models;

namespace ImportBL
{
    public class FileReader : IFileReader
    {
        private readonly ILogger _logger;

        public FileReader(ILogger logger)
        {
            _logger = logger;
        }

        public List<Gift> ReadGifts(string filePath)
        {
            var table = ReadFromFile(filePath).Tables[0];
            var gifts = new List<Gift>();
            int rowCounter = 2;

            foreach (DataRow row in table.Rows)
            {
                try
                {
                    string date = row[0].ToString()?.Replace("za ", "01.01.");

                    var gift = new Gift
                    {
                        DatumDaru = string.IsNullOrWhiteSpace(date) ? default : Convert.ToDateTime(date),
                        Aktivity = row[1].ToString(),
                        KontaktEmail = row[2].ToString(),
                        ContactName = row[3].ToString(),
                        ContactSurname = row[4].ToString(),
                        Castka = Convert.ToDecimal(row[5].ToString()?.Replace('.', ',')),
                        CisloUctu = row[6].ToString(),
                        KodBanky = row[7].ToString(),
                        PrisloNaUcet = row[8].ToString(),
                        VariabilniSymbol = row[9] == null ? Convert.ToInt32(row[8]?.ToString()) : (int?) null,
                        PlatebniMetoda = row[10].ToString(),
                        StavPlatby = row[11].ToString(),
                        PotvrzeniKDaru = row[12].ToString(),
                        PoznamkaKDaru = row[13].ToString(),
                        SpecifickySymbol = row[14].ToString(),
                        ZdrojDaru = row[15].ToString(),
                        Ucel = row[16].ToString(),
                        Ocisteny = row[17].ToString(),
                        
                        Row = rowCounter++
                    };
                    gifts.Add(gift);
                }
                catch (Exception e)
                {
                    _logger.LogException(e);
                }
            }

            return gifts;
        }

        private DataSet ReadFromFile(string path)
        {
            try
            {
                using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var reader = ExcelReaderFactory.CreateReader(stream);

                // reader.IsFirstRowAsColumnNames
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true 
                    }
                };

                return reader.AsDataSet(conf);
            }
            catch (Exception e)
            {
                throw new LocalException(e.Message);
            }
        }
    }
}
