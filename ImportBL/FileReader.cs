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
        public List<Gift> ReadGifts(string filePath)
        {
            var table = ReadFromFile(filePath).Tables[0];
            var gifts = new List<Gift>();
            int rowCounter = 2;

            foreach (DataRow row in table.Rows)
            {
                try
                {
                    string date = row[7].ToString()?.Replace("za ", "01.01.");

                    var gift = new Gift
                    {
                        KontaktEmail = row[0].ToString(),
                        SubjektId = row[1].ToString(),
                        Castka = Convert.ToDecimal(row[2].ToString()?.Replace('.', ',')),
                        CisloUctu = row[3].ToString(),
                        KodBanky = row[4].ToString(),
                        VariabilniSymbol = row[5] == null ? Convert.ToInt32(row[8]?.ToString()) : (int?) null,
                        PoznamkaKDaru = row[6].ToString(),
                        DatumDaru = string.IsNullOrWhiteSpace(date) ? default : Convert.ToDateTime(date),
                        PrisloNaUcet = row[8].ToString(),

                        ZdrojDaru = row[10].ToString(),

                        Row = rowCounter++
                    };
                    gifts.Add(gift);
                }
                catch (Exception e)
                {
                    //throw new LocalException("DataReader", $"Gift at line {rowCounter} wasn't read", e.Message);
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
                //throw new LocalException("FileReader", "Error occurred while reading Gifts from Excel document", e.Message);
            }

            return null;
        }
    }
}
