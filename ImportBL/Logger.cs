using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImportBL.Exceptions;
using ImportBL.Interfaces;

namespace ImportBL
{
    public class Logger : ILogger
    {
        public List<string> ErrorList { get; set; } = new List<string>();
        public List<string> NewErrors { get; set; } = new List<string>();

        public event EventHandler StateChanged;

        public void LogInfo(string message)
        {
            InsertInfo(message);

            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void LogException(LocalException exception)
        {
            foreach (var error in exception.ErrorList)
            {
                InsertError(error);
            }

            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void LogException(Exception exception)
        {
            InsertError(exception.Message);

            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void LogException(string message)
        {
            InsertError(message);

            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void LogException(List<string> messages)
        {
            if (messages.Count == 0)
            {
                return;
            }

            foreach (var message in messages)
            {
                InsertError(message);
            }

            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public string GetNewLogs()
        {
            string res = string.Empty;

            foreach (var error in NewErrors)
            {
                res += error + "\n\n";
            }

            NewErrors.Clear();

            return res;
        }

        public async Task LogToFile(string filePath)
        {
            string content = string.Empty;

            foreach (var error in ErrorList)
            {
                content += error + "\n\n";
            }

            ErrorList.Clear();

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await File.WriteAllTextAsync(filePath, content);
        }

        private void InsertError(string message)
        {
            var errorToLog = "error: " + message;

            ErrorList.Add(errorToLog);
            NewErrors.Add(errorToLog);
        }

        private void InsertInfo(string message)
        {
            var errorToLog = "info: " + message;

            ErrorList.Add(errorToLog);
            NewErrors.Add(errorToLog);
        }
    }
}
