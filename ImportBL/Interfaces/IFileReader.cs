using System.Collections.Generic;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    interface IFileReader
    {
        IEnumerable<Gift> ReadGifts(string filePath);
    }
}
