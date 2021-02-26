using System;
using System.Collections.Generic;
using System.Linq;
using ImportBL.Interfaces;
using ImportBL.Models;

namespace ImportBL
{
    public class DataPair : IDataPair
    {
        private readonly ILogger _logger;

        public DataPair(ILogger logger)
        {
            _logger = logger;
        }

        public List<Contact> ConnectData(List<Gift> gifts, List<Contact> contacts, List<Subject> subjects)
        {
            var contactsToInsert = new List<Contact>();

            foreach (var gift in gifts)
            {
                try
                {
                    // connect by database ID
                    if (!string.IsNullOrWhiteSpace(gift.Kontakt?.Id))
                    {
                        gift.Kontakt = contacts.SingleOrDefault(c => c.Id == gift.Kontakt.Id);
                    }

                    // connect by email
                    if (!string.IsNullOrWhiteSpace(gift.KontaktEmail) && gift.Kontakt == null)
                    {
                        gift.Kontakt = contacts.SingleOrDefault(c => c.Email?.Href == gift.KontaktEmail);
                    }

                    // connect by account number
                    if (!string.IsNullOrWhiteSpace(gift.CisloUctu) && gift.Kontakt == null)
                    {
                        gift.Kontakt = contacts.SingleOrDefault(c => c.CisloUctu == gift.CisloUctu);
                    }

                    // connect by specific number
                    if (!string.IsNullOrWhiteSpace(gift.SpecifickySymbol) && gift.Kontakt == null)
                    {
                        gift.Kontakt = contacts.SingleOrDefault(c => c.SpecifickySymbol == gift.SpecifickySymbol);
                    }

                    // create new contact
                    if (gift.Kontakt == null)
                    {
                        gift.Kontakt = new Contact
                        {
                            Email = new Url {Href = gift.KontaktEmail},
                            CisloUctu = gift.CisloUctu,
                            SpecifickySymbol = gift.SpecifickySymbol,
                            Prijmeni = gift.ContactSurname,
                            Jmeno = gift.ContactName
                        };

                        contactsToInsert.Add(gift.Kontakt);
                        contacts.Add(gift.Kontakt);
                    }

                    // add current gift to list of contact's gifts
                    gift.Kontakt.Gifts.Add(gift);

                    if (!string.IsNullOrWhiteSpace(gift.SubjektId))
                    {
                        gift.Subjekt = subjects.SingleOrDefault(s => s.Ico == gift.SubjektId);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogException($"{e.Message}, an error occurred while pairing gift: \n" + gift);
                }
            }

            return contactsToInsert;
        }
    }
}
