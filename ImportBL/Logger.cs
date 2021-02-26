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
        /// <summary>
        /// Whole error list
        /// </summary>
        private List<string> ErrorList { get; } = new List<string>();
        /// <summary>
        /// Only new errors
        /// </summary>
        private List<string> CurrentErrors { get; } = new List<string>();

        public event EventHandler StateChanged;
        public int SuccessfullyGiftsRead { get; set; }
        public int SuccessfullyContactsGot { get; set; }
        public int SuccessfullySubjectsGot { get; set; }
        public int SuccessfullyGiftsGot { get; set; }
        public int SuccessfullyGiftsPaired { get; set; }
        public int SuccessfullyItemsSent { get; set; }
        public int SuccessfullyContactsUpdated { get; set; }
        public int ErroneousGiftsPaired { get; set; }
        public int ErroneousItemsSent { get; set; }
        public int ErroneousContactsUpdated { get; set; }

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

            foreach (var error in CurrentErrors)
            {
                res += error + "\n\n";
            }

            CurrentErrors.Clear();

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
            CurrentErrors.Add(errorToLog);
        }

        private void InsertInfo(string message)
        {
            var errorToLog = "info: " + message;

            ErrorList.Add(errorToLog);
            CurrentErrors.Add(errorToLog);
        }
    }
}
