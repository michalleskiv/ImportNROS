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

            try
            {
                foreach (DataRow row in table.Rows)
                {
                    string date = row[0].ToString()?.Replace("za ", "01.01.");
                        
                    var gift = new Gift
                    {
                        DatumDaru = string.IsNullOrWhiteSpace(date) ? default : Convert.ToDateTime(date),
                        KontaktEmail = row[2].ToString(),
                        ContactName = row[3].ToString(),
                        ContactSurname = row[4].ToString(),
                        Castka = Convert.ToDecimal(row[5].ToString()?.Replace('.', ',')),
                        CisloUctu = row[6].ToString(),
                        KodBanky = row[7].ToString(),
                        PrisloNaUcet = row[8].ToString(),
                        VariabilniSymbol = !string.IsNullOrWhiteSpace(row[9]?.ToString()) ? Convert.ToInt64(row[9]?.ToString()) : (long?) null,
                        PlatebniMetoda = row[10].ToString(),
                        StavPlatby = row[11].ToString(),
                        PotvrzeniKDaru = row[12].ToString(),
                        PoznamkaKDaru = row[13].ToString(),
                        SS = !string.IsNullOrWhiteSpace(row[14]?.ToString()) ? Convert.ToInt64(row[14]?.ToString()) : (long?) null,
                        ZdrojDaru = row[15].ToString(),
                        UcelDaru = row[16].ToString(),
                        DarOcisteny = !string.IsNullOrWhiteSpace(row[17]?.ToString()) ? Convert.ToInt32(row[17]?.ToString()) : (int?) null,
                        
                        Row = rowCounter++
                    };
                    gifts.Add(gift);
                }
            }
            catch (Exception e)
            {
                throw new LocalException($"Input file is not correct, row: {rowCounter}, message: {e.Message}");
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
