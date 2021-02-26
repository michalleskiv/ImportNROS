using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImportBL.Exceptions;

namespace ImportBL.Interfaces
{
    /// <summary>
    /// Logs exceptions and other important information
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Invokes if log list appends
        /// </summary>
        event EventHandler StateChanged;

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

        void LogInfo(string message);
        void LogException(LocalException exception);
        void LogException(Exception exception);
        void LogException(string message);
        void LogException(List<string> messages);

        /// <summary>
        /// Get only new logs
        /// </summary>
        /// <returns></returns>
        string GetNewLogs();

        /// <summary>
        /// Write all logs to a file
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns></returns>
        Task LogToFile(string filePath);
    }
}
