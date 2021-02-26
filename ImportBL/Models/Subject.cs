namespace ImportBL.Models
{
    public class Subject : Item
    {
        public string CisloUctu { get; set; }

        public override bool Equals(Item other)
        {
            if (!(other is Subject))
            {
                return false;
            }

            var subject = (Subject) other;

            if (!string.IsNullOrWhiteSpace(CisloUctu) && !string.IsNullOrWhiteSpace(subject.CisloUctu)
                                                && CisloUctu == subject.CisloUctu)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return "Subject: " +
                   $"\t-ico: {CisloUctu}";
        }
    }
}
