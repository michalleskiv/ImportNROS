using System.Collections.Generic;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    /// <summary>
    /// Connects gifts with other data
    /// </summary>
    public interface IDataPair
    {
        /// <summary>
        /// Pair gifts with contacts and subjects
        /// </summary>
        /// <param name="gifts">List of gifts</param>
        /// <param name="contacts">List of contacts</param>
        /// <param name="subjects">List of subjects</param>
        /// <returns>List of new contacts to add to Tabidoo</returns>
        List<Contact> ConnectDataExcel(List<Gift> gifts, List<Contact> contacts, List<Subject> subjects);
        void ConnectDataTabidoo(List<Gift> gifts, List<Contact> contacts);
    }
}
