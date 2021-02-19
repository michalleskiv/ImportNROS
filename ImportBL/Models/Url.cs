namespace ImportBL.Models
{
    public class Url
    {
        public string Href { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Href;
        }
    }
}
