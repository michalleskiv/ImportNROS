using System;

namespace ImportBL.Models
{
    public class Gift : Item
    {
        public string KontaktEmail { get; set; }
        public string SpecifickySymbol { get; set; }
        public string ContactName { get; set; }
        public string ContactSurname { get; set; }
        public Contact Kontakt { get; set; }
        public string SubjektId { get; set; }
        public Subject Subjekt { get; set; }
        public string PlatebniMetoda { get; set; }
        public string PotvrzeniKDaru { get; set; }
        public string StavPlatby { get; set; }
        public string Ucel { get; set; }
        public decimal? Castka { get; set; }
        public string CisloUctu { get; set; }
        public string KodBanky { get; set; }
        public int? VariabilniSymbol { get; set; }
        public string PoznamkaKDaru { get; set; }
        public DateTime DatumDaru { get; set; }
        public string PrisloNaUcet { get; set; }
        public string ZdrojDaru { get; set; }
        public string Aktivity { get; set; }
        public string Ocisteny { get; set; }

        public int Row { get; set; }
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
                   $"\t-ss: {SpecifickySymbol}\n" +
                   $"\t-id: {Id}\n" +
                   $"-row (if gift was in Excel): {Row}";
        }
    }
}
