using System;
using System.Collections.Generic;
using System.Text;
using ImportBL.Interfaces;

namespace ImportBL.Models
{
    public class Logger : ILogger
    {
        public void LogInfo(string message)
        {
            throw new NotImplementedException();
        }

        public void LogException(string message)
        {
            throw new NotImplementedException();
        }

        public void LogException(string message, string originalMessage)
        {
            throw new NotImplementedException();
        }
    }
}
