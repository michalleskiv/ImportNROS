using System;
using System.Collections.Generic;
using System.Linq;
using ImportBL.Interfaces;
using ImportBL.Models;

namespace ImportBL
{
    public class DataPair : IDataPair
    {
        public List<Contact> ConnectData(List<Gift> gifts, List<Contact> contacts, List<Subject> subjects)
        {
            var contactsToInsert = new List<Contact>();

            foreach (var gift in gifts)
            {
                if (gift.Kontakt == null)
                {
                    if (!string.IsNullOrWhiteSpace(gift.KontaktEmail))
                    {
                        gift.Kontakt = contacts.SingleOrDefault(c => c.Email?.Href == gift.KontaktEmail);
                    }

                    if (!string.IsNullOrWhiteSpace(gift.CisloUctu) && gift.Kontakt == null)
                    {
                        gift.Kontakt = contacts.SingleOrDefault(c => c.CisloUctu == gift.CisloUctu);
                    }

                    if (!string.IsNullOrWhiteSpace(gift.SpecifickySymbol) && gift.Kontakt == null)
                    {
                        gift.Kontakt = contacts.SingleOrDefault(c => c.SpecifickySymbol == gift.SpecifickySymbol);
                    }

                    if (gift.Kontakt == null)
                    {
                        gift.Kontakt = new Contact
                        {
                            Email = new Url {Href = gift.KontaktEmail},
                            CisloUctu = gift.CisloUctu,
                            SpecifickySymbol = gift.SpecifickySymbol
                        };

                        contactsToInsert.Add(gift.Kontakt);
                        contacts.Add(gift.Kontakt);
                    }
                }

                if (!string.IsNullOrWhiteSpace(gift.SubjektId))
                {
                    gift.Subjekt = subjects.SingleOrDefault(s => s.Ico == gift.SubjektId);
                }
            }

            return contactsToInsert;
        }
    }
}
