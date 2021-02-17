using System.Collections.Generic;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    interface IDataPair
    {
        void ConnectData(List<Gift> gifts, List<Contact> contacts, List<Subject> subjects);
    }
}
