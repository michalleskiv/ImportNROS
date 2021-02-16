using System.Collections.Generic;
using System.Threading.Tasks;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    interface ITagsProcessor
    {
        void UpdateTagsLocal(IEnumerable<Contact> contacts);
        Task UpdateTagsTabidoo(IEnumerable<Contact> contacts);
    }
}
