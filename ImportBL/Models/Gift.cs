using System;
using System.Collections.Generic;
using System.Text;

namespace ImportBL.Models
{
    public class Gift : Item
    {
        public string KontaktId { get; set; }
        public Contact Kontakt { get; set; }
        public string SubjektId { get; set; }
        public Subject Subjekt { get; set; }
        public List<string> Kampan { get; set; }
        public string PlatebniMetoda { get; set; }
        public string StavPlatby { get; set; }
        public decimal? Castka { get; set; }
        public string CisloUctu { get; set; }
        public string KodBanky { get; set; }
        public int? VariabilniSymbol { get; set; }
        public string PoznamkaKDaru { get; set; }
        public DateTime DatumDaru { get; set; }
        public string PrisloNaUcet { get; set; }
        public string Poznamka { get; set; }
        public string ZdrojDaru { get; set; }

        public int Row { get; set; }
        public string ImportId { get; set; }
    }
}
