using System.Collections.Generic;
using Newtonsoft.Json;

namespace ImportBL.Models
{
    public class Contact : Item
    {
        public Url Email { get; set; }
        public string CisloUctu { get; set; }
        public string SpecifickySymbol { get; set; }
        public List<string> Tag { get; set; }
        [JsonIgnore]
        public List<Gift> Gifts { get; set; } = new List<Gift>();

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

        public override string ToString()
        {
            return "Contact:\n" +
                   $"\t-email: {Email}\n" +
                   $"\t-cislo uctu: {CisloUctu}\n" +
                   $"\t-ss: {SpecifickySymbol}\n" +
                   $"\t-id: {Id}";
        }
    }
}
