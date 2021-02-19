using System;
using System.Threading.Tasks;

namespace ImportBL.Interfaces
{
    public interface IMainWorker
    {
        public double Progress { get; set; }
        public bool IsFree { get; set; }

        public event EventHandler StateChanged;

        Task Run(string filePath);
        string GetNewLogs();
    }
}
