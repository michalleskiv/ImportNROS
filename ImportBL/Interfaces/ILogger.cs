using System;
using System.Collections.Generic;
using System.Text;

namespace ImportBL.Interfaces
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogException(string message);
        void LogException(string message, string originalMessage);
    }
}
