using System;
using Newtonsoft.Json;

namespace ImportBL.Models
{
    public class Gift : Item
    {
        public string KontaktEmail { get; set; }
        [JsonIgnore]
        public long? SS { get; set; }
        [JsonIgnore]
        public string ContactName { get; set; }
        [JsonIgnore]
        public string ContactSurname { get; set; }
        public Contact Kontakt { get; set; }
        [JsonIgnore]
        public string SubjektId { get; set; }
        public Subject Subjekt { get; set; }
        public string PlatebniMetoda { get; set; }
        public string PotvrzeniKDaru { get; set; }
        public string StavPlatby { get; set; }
        public string UcelDaru { get; set; }
        public decimal? Castka { get; set; }
        public string CisloUctu { get; set; }
        public string KodBanky { get; set; }
        public long? VariabilniSymbol { get; set; }
        public string PoznamkaKDaru { get; set; }
        public DateTime DatumDaru { get; set; }
        public string PrisloNaUcet { get; set; }
        public string ZdrojDaru { get; set; }
        public double? DarOcisteny { get; set; }

        public int Row { get; set; }
        [JsonProperty(PropertyName = "importID")]
        public string ImportId { get; set; }

        public override bool Equals(Item other)
        {
            return false;
        }

        public override string ToString()
        {
            return "Gift: \n" +
                   $"\t-email: {KontaktEmail}\n" +
                   $"\t-cislo uctu: {CisloUctu}\n" +
                   $"\t-ss: {SS}\n" +
                   $"\t-id: {Id}\n" +
                   $"-row (if gift was in Excel): {Row}";
        }
    }
}
