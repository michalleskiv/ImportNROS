using System.Collections.Generic;
using System.Threading.Tasks;
using ImportBL.Models;
using ImportBL.Models.Dto;

namespace ImportBL.Interfaces
{
    interface IDataSender
    {
        Task SendContacts(IEnumerable<Contact> contacts);
        Task SendGifts(IEnumerable<Gift> gifts);
        Task UpdateContacts(IEnumerable<ContactUpdateDto> contacts);
    }
}
