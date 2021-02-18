using System.Collections.Generic;

namespace ImportBL.Models
{
    public class Contact : Item
    {
        public Url Email { get; set; }
        public string CisloUctu { get; set; }
        public string SpecifickySymbol { get; set; }
        public List<string> Tag { get; set; }

        public override bool Equals(Item other)
        {
            if (!(other is Contact))
            {
                return false;
            }

            var contact = (Contact) other;
            
            if (!string.IsNullOrWhiteSpace(Email?.Href) && !string.IsNullOrWhiteSpace(contact.Email?.Href) 
                                                        && Email?.Href == contact.Email?.Href )
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(CisloUctu) && !string.IsNullOrWhiteSpace(contact.CisloUctu) 
                                                      && CisloUctu == contact.CisloUctu)
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(SpecifickySymbol) && !string.IsNullOrWhiteSpace(contact.SpecifickySymbol) 
                                                             && SpecifickySymbol == contact.SpecifickySymbol)
            {
                return true;
            }

            return false;
        }
    }
}
