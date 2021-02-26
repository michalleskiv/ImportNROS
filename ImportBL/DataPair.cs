using System;
using System.Collections.Generic;
using System.Linq;
using ImportBL.Exceptions;
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

        public List<Contact> ConnectDataExcel(List<Gift> gifts, List<Contact> contacts, List<Subject> subjects)
        {
            var contactsToInsert = new List<Contact>();

            foreach (var gift in gifts)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(gift.KontaktEmail) && gift.SS == null &&
                        !string.IsNullOrWhiteSpace(gift.CisloUctu))
                    {
                        ConnectWithContacts(gift, contacts);

                        if (gift.Kontakt == null)
                        {
                            ConnectWithSubjects(gift, subjects);

                            if (gift.Subjekt == null)
                            {
                                _logger.ErroneousGiftsPaired++;
                                throw new LocalException($"Gift {gift} cannot be connected with contact or subject");
                            }
                        }

                        _logger.SuccessfullyGiftsPaired++;
                    }
                    else
                    {
                        ConnectWithContacts(gift, contacts);
                        CreateContact(gift, contacts, contactsToInsert);
                        _logger.SuccessfullyGiftsPaired++;
                    }
                }
                catch (LocalException e)
                {
                    _logger.LogException(e);
                }
                catch (Exception e)
                {
                    _logger.LogException($"{e.Message}, an error occurred while pairing gift: \n" + gift);
                }
            }

            return contactsToInsert;
        }

        public void ConnectDataTabidoo(List<Gift> gifts, List<Contact> contacts)
        {
            foreach (var gift in gifts)
            {
                ConnectWithContacts(gift, contacts);
            }
        }

        private void ConnectWithContacts(Gift gift, List<Contact> contacts)
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

            // connect by specific number
            if (gift.SS != null && gift.Kontakt == null)
            {
                gift.Kontakt = contacts.SingleOrDefault(c => c.SS == gift.SS);
            }

            // connect by account number
            if (!string.IsNullOrWhiteSpace(gift.CisloUctu) && gift.Kontakt == null)
            {
                gift.Kontakt = contacts.SingleOrDefault(c => c.CisloUctu == gift.CisloUctu);
            }

            // add current gift to list of contact's gifts
            gift.Kontakt?.Gifts.Add(gift);
        }

        private void CreateContact(Gift gift, List<Contact> contacts, List<Contact> contactsToInsert)
        {
            // create new contact
            if (gift.Kontakt == null)
            {
                gift.Kontakt = new Contact
                {
                    Email = !string.IsNullOrWhiteSpace(gift.KontaktEmail) ? new Url {Href = gift.KontaktEmail} : null,
                    CisloUctu = !string.IsNullOrWhiteSpace(gift.CisloUctu) ? gift.CisloUctu : null,
                    SS = gift.SS,
                    Prijmeni = gift.ContactSurname,
                    Jmeno = gift.ContactName
                };

                contactsToInsert.Add(gift.Kontakt);
                contacts.Add(gift.Kontakt);
            }
        }

        private void ConnectWithSubjects(Gift gift, List<Subject> subjects)
        {
            if (!string.IsNullOrWhiteSpace(gift.CisloUctu))
            {
                gift.Subjekt = subjects.SingleOrDefault(s => s.CisloUctu == gift.CisloUctu);
            }
        }
    }
}
