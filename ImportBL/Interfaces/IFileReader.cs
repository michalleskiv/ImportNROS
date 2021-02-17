using System.Collections.Generic;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    public interface IFileReader
    {
        IEnumerable<Gift> ReadGifts(string filePath);
    }
}
