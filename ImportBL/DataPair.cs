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
                    if (!string.IsNullOrWhiteSpace(gift.Kontakt?.Id))
                    {
                        gift.Kontakt = contacts.SingleOrDefault(c => c.Id == gift.Kontakt.Id);
                    }

                    if (!string.IsNullOrWhiteSpace(gift.KontaktEmail) && gift.Kontakt == null)
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

                    gift.Kontakt.Gifts.Add(gift);

                    if (!string.IsNullOrWhiteSpace(gift.SubjektId))
                    {
                        gift.Subjekt = subjects.SingleOrDefault(s => s.Ico == gift.SubjektId);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogException($"{e.Message}, an error occurred while pairing gift: \n" +
                                         $"-email: {gift.KontaktEmail},\n" +
                                         $"-cislo uctu: {gift.CisloUctu},\n" +
                                         $"-ss: {gift.SpecifickySymbol},\n" +
                                         $"-castka: {gift.Castka},\n" +
                                         $"-row (if gift was in Excel): {gift.Row}");
                }
            }

            return contactsToInsert;
        }
    }
}
