using System.Collections.Generic;
using System.Threading.Tasks;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    /// <summary>
    /// Sends data to Tabidoo
    /// </summary>
    public interface IDataSender
    {
        /// <summary>
        /// Send list of items to Tabidoo
        /// </summary>
        /// <typeparam name="T">Contact Subject Gift</typeparam>
        /// <param name="schemaId">ID of schema where items should be send</param>
        /// <param name="items">List of items</param>
        /// <returns></returns>
        Task SendItems<T>(string schemaId, List<T> items) where T: Item;

        /// <summary>
        /// Update contacts' tags
        /// </summary>
        /// <param name="contactsSchemaId">Contacts schema ID</param>
        /// <param name="contacts">List of contacts</param>
        /// <returns></returns>
        Task UpdateContacts(string contactsSchemaId, List<Contact> contacts);
    }
}
