using System.Collections.Generic;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    interface IDataConnector
    {
        void ConnectData(IEnumerable<Contact> contacts, IEnumerable<Gift> gifts);
    }
}
