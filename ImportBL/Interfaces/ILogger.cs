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
