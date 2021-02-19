using System;
using System.Threading.Tasks;

namespace ImportBL.Interfaces
{
    /// <summary>
    /// Manages all classes in the library
    /// </summary>
    public interface IMainWorker
    {
        /// <summary>
        /// Using to control progress bar
        /// </summary>
        public double Progress { get; set; }
        /// <summary>
        /// Is worker free now
        /// </summary>
        public bool IsFree { get; set; }

        /// <summary>
        /// Invokes if log list appends
        /// </summary>
        public event EventHandler StateChanged;

        /// <summary>
        /// Run calculations
        /// </summary>
        /// <param name="filePath">Path to Excel file</param>
        /// <returns></returns>
        Task Run(string filePath);

        /// <summary>
        /// Get new logs
        /// </summary>
        /// <returns>New logs</returns>
        string GetNewLogs();
    }
}
