namespace ImportBL.Models
{
    public class Subject : Item
    {
        public string Ico { get; set; }

        public override bool Equals(Item other)
        {
            if (!(other is Subject))
            {
                return false;
            }

            var subject = (Subject) other;

            if (!string.IsNullOrWhiteSpace(Ico) && !string.IsNullOrWhiteSpace(subject.Ico)
                                                && Ico == subject.Ico)
            {
                return true;
            }

            return false;
        }
    }
}
