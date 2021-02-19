using System.Collections.Generic;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    public interface IContactUpdater
    {
        void UpdateTags(List<Contact> contacts, List<Gift> gifts);
    }
}
