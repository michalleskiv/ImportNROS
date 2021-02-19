using System.Collections.Generic;
using System.Threading.Tasks;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    /// <summary>
    /// Receives data from Tabidoo
    /// </summary>
    public interface IDataReceiver
    {
        /// <summary>
        /// Receive list of items from Tabidoo
        /// </summary>
        /// <typeparam name="T">Contact Subject Gift</typeparam>
        /// <param name="schemaId">Schema ID</param>
        /// <returns>List of items</returns>
        Task<List<T>> GetTable<T>(string schemaId) where T: Item;
    }
}
