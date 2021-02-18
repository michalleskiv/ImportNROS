using System.Collections.Generic;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    public interface ISessionIdGenerator
    {
        public string Id { get; }

        void MarkGifts(List<Gift> gifts);
    }
}
