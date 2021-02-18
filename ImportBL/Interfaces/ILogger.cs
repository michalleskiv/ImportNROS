using System;
using System.Threading.Tasks;
using ImportBL.Exceptions;

namespace ImportBL.Interfaces
{
    public interface ILogger
    {
        void LogException(LocalException exception);
        void LogException(Exception exception);
        public Task LogToFile(string filePath);
    }
}
