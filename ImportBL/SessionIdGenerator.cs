using System;
using System.Collections.Generic;
using ImportBL.Interfaces;
using ImportBL.Models;

namespace ImportBL
{
    public class SessionIdGenerator : ISessionIdGenerator
    {
        public string Id { get; }

        public SessionIdGenerator()
        {
            Id = GenerateId();
        }

        public void MarkGifts(List<Gift> gifts)
        {
            gifts.ForEach(g => g.ImportId = Id);
        }

        private string GenerateId()
        {
            var random = new Random();
            return DateTime.Now.ToString("yyMMddHHmm") + random.Next(1000).ToString("000");
        }
    }
}
