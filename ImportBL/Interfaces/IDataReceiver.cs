using System.Collections.Generic;
using System.Threading.Tasks;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    public interface IDataReceiver
    {
        Task<IEnumerable<T>> GetTable<T>(string schemaId) where T: Item;
    }
}
