using System.Collections.Generic;
using Newtonsoft.Json;

namespace ImportBL.Models
{
    public class Contact : Item
    {
        public Url Email { get; set; }
        public string CisloUctu { get; set; }
        [JsonProperty(PropertyName = "SS")]
        public long? SS { get; set; }
        public string Prijmeni { get; set; }
        public string Jmeno { get; set; }
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

            if (SS != null && contact.SS != null && SS == contact.SS)
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
                   $"\t-ss: {SS}\n" +
                   $"\t-id: {Id}";
        }
    }
}
