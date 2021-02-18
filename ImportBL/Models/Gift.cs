﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ImportBL.Models
{
    public class Gift : Item
    {
        public string KontaktEmail { get; set; }
        public string SpecifickySymbol { get; set; }
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

        public override bool Equals(Item other)
        {
            if (!(other is Gift))
            {
                return false;
            }

            var gift = (Gift) other;

            return false;
        }
    }
}
