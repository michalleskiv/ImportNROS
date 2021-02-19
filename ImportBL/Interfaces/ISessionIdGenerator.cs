using System.Collections.Generic;
using ImportBL.Models;

namespace ImportBL.Interfaces
{
    /// <summary>
    /// Use to create and keep ID of current data import
    /// </summary>
    public interface ISessionIdGenerator
    {
        /// <summary>
        /// Current ID
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Add ID to every gift in the list
        /// </summary>
        /// <param name="gifts">List of gifts</param>
        void MarkGifts(List<Gift> gifts);
    }
}
