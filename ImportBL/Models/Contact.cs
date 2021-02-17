using System;
using System.Collections.Generic;
using System.Text;

namespace ImportBL.Models
{
    public class Contact : Item
    {
        public Url Email { get; set; }
        public string CisloUctu { get; set; }
        public string SpecifickySymbol { get; set; }
        public List<string> Tag { get; set; }
    }
}
