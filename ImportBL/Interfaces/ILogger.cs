using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImportBL.Exceptions;

namespace ImportBL.Interfaces
{
    public interface ILogger
    {
        event EventHandler StateChanged;

        void LogInfo(string message);
        void LogException(LocalException exception);
        void LogException(Exception exception);
        void LogException(string message);
        void LogException(List<string> messages);
        string GetNewLogs();
        Task LogToFile(string filePath);
    }
}
