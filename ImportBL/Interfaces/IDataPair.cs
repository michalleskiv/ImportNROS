﻿using System.Collections.Generic;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    interface IDataPair
    {
        List<Contact> ConnectData(List<Gift> gifts, List<Contact> contacts, List<Subject> subjects);
    }
}
