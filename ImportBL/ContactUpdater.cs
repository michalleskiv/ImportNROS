using System;
using System.Collections.Generic;
using System.Linq;
using ImportBL.Interfaces;
using ImportBL.Models;

namespace ImportBL
{
    public class ContactUpdater : IContactUpdater
    {
        private const string _prvodarce = "Prvodárce";
        private const string _individualni = "Individuální dárce";
        private const string _pravidelny = "Pravidelný dárce";
        private const string _klubar = "Klubař";
        private const string _cisloUctu = "12301230/0600";

        public void UpdateTags(List<Contact> contacts, List<Gift> gifts)
        {
            UpdateNormal(contacts);
            UpdateKlubar(gifts);
        }

        private void UpdateNormal(List<Contact> contacts)
        {
            foreach (var contact in contacts)
            {
                if (contact.Tag != null)
                {
                    contact.Tag.Remove(_prvodarce);
                    contact.Tag.Remove(_individualni);
                    contact.Tag.Remove(_pravidelny);
                    contact.Tag.Remove(_klubar);
                }
                else
                {
                    contact.Tag = new List<string>();
                }

                var giftsToCount = contact.Gifts.Where(g =>
                    {
                        var start = DateTime.Now.AddYears(-1);
                        start = start.AddMonths(1);
                        start = new DateTime(start.Year, start.Month, 1);

                        return start < g.DatumDaru && g.DatumDaru < DateTime.Now;
                    })
                    .ToList();

                if (0 <= giftsToCount.Count && giftsToCount.Count <= 1)
                {
                    contact.Tag.Add(_prvodarce);
                }
                else if (2 <= giftsToCount.Count && giftsToCount.Count <= 3)
                {
                    contact.Tag.Add(_individualni);
                }
                else if (4 <= giftsToCount.Count)
                {
                    contact.Tag.Add(_pravidelny);
                }
            }
        }

        private void UpdateKlubar(List<Gift> gifts)
        {
            var giftsToCheck = gifts.Where(g => g.CisloUctu == _cisloUctu);

            foreach (var gift in giftsToCheck)
            {
                gift.Kontakt.Tag.Clear();
                gift.Kontakt.Tag.Add(_klubar);
            }
        }
    }
}
