﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    public interface IDataSender
    {
        Task SendItems<T>(string schemaId, List<T> items) where T: Item;
    }
}
