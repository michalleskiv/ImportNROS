using System;
using System.Collections.Generic;
using System.Linq;
using ImportBL.Interfaces;
using ImportBL.Models;

namespace ImportBL
{
    public class ContactUpdater : IContactUpdater
    {
        public void UpdateTags(List<Contact> contacts, List<Gift> gifts)
        {
            UpdateNormal(contacts);
            UpdateKlubar(gifts);
        }

        private void UpdateNormal(List<Contact> contacts)
        {
            foreach (var contact in contacts)
            {
                contact.Tag = new List<string>();

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
                    contact.Tag.Add("Prvodárce");
                }
                else if (2 <= giftsToCount.Count && giftsToCount.Count <= 3)
                {
                    contact.Tag.Add("Individuální dárce");
                }
                else if (4 <= giftsToCount.Count)
                {
                    contact.Tag.Add("Pravidelný dárce");
                }
            }
        }

        private void UpdateKlubar(List<Gift> gifts)
        {
            var giftsToCheck = gifts.Where(g => g.CisloUctu == "12301230/0600");

            foreach (var gift in giftsToCheck)
            {
                gift?.Kontakt.Tag.Clear();
                gift?.Kontakt.Tag.Add("Klubař");
            }
        }
    }
}
