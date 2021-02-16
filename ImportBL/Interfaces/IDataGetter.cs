using System.Collections.Generic;
using System.Threading.Tasks;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    interface IDataGetter
    {
        Task<IEnumerable<Contact>> GetContacts();
        Task<IEnumerable<Gift>> GetGifts();
    }
}
