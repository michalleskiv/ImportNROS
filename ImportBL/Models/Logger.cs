using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImportBL.Exceptions;
using ImportBL.Interfaces;

namespace ImportBL.Models
{
    public class Logger : ILogger
    {
        public List<string> ErrorList { get; set; } = new List<string>();
        public List<string> NewErrors { get; set; } = new List<string>();

        public void LogException(LocalException exception)
        {
            ErrorList.AddRange(exception.ErrorList);
            NewErrors.AddRange(exception.ErrorList);
        }

        public void LogException(Exception exception)
        {
            ErrorList.Add(exception.Message);
            NewErrors.Add(exception.Message);
        }

        public async Task LogToFile(string filePath)
        {
            string content = string.Empty;

            foreach (var error in ErrorList)
            {
                content += error + "\n\n";
            }

            await File.WriteAllTextAsync(filePath, content);
        }
    }
}
