using System.Collections.Generic;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    /// <summary>
    /// Use only for update contacts tags
    /// </summary>
    public interface IContactUpdater
    {
        /// <summary>
        /// Update contacts' tags considering count of gifts
        ///     - 0-1 gifts: Prvodarce
        ///     - 2-3 gifts: Individualni darce
        ///     - 4 and more gifts: Pravidelny darce
        /// </summary>
        /// <param name="contacts">List of contacts</param>
        /// <param name="gifts">List of gifts</param>
        void UpdateTags(List<Contact> contacts, List<Gift> gifts);
    }
}
