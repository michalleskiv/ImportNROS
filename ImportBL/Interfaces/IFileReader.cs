using System.Collections.Generic;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    /// <summary>
    /// Reads data from Excel file
    /// </summary>
    public interface IFileReader
    {
        /// <summary>
        /// Read gifts from Excel file
        /// </summary>
        /// <param name="filePath">Path to the Excel file</param>
        /// <returns>List of read gifts</returns>
        List<Gift> ReadGifts(string filePath);
    }
}
