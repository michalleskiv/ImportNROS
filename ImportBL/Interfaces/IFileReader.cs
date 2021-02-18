using System.Collections.Generic;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    public interface IFileReader
    {
        List<Gift> ReadGifts(string filePath);
    }
}
