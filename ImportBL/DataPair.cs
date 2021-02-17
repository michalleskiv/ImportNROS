using System.Collections.Generic;
using System.Linq;
using ImportBL.Interfaces;
using ImportBL.Models;

namespace ImportBL
{
    public class DataPair : IDataPair
    {
        public void ConnectData(List<Gift> gifts, List<Contact> contacts, List<Subject> subjects)
        {
            foreach (var gift in gifts)
            {
                if (gift.Kontakt == null)
                {
                    if (!string.IsNullOrWhiteSpace(gift.KontaktId))
                    {
                        gift.Kontakt = contacts.SingleOrDefault(c => c.Email?.Href == gift.KontaktId);
                    }

                    if (!string.IsNullOrWhiteSpace(gift.CisloUctu) && gift.Kontakt == null)
                    {
                        gift.Kontakt = contacts.SingleOrDefault(c => c.CisloUctu == gift.CisloUctu);
                    }
                }

                if (!string.IsNullOrWhiteSpace(gift.SubjektId))
                {
                    gift.Subjekt = subjects.SingleOrDefault(s => s.Ico == gift.SubjektId);
                }
            }
        }
    }
}
