using System.Collections.Generic;

namespace ImportBL.Models.Dto
{
    public class ContactUpdateDto
    {
        public List<string> Tag { get; set; }

        public ContactUpdateDto(Contact contact)
        {
            Tag = contact.Tag;
        }
    }
}
