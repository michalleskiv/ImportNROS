using System.Collections.Generic;

namespace ImportBL.Models.Dto
{
    public class ItemsHolder<T> where T: Item
    {
        public List<ItemHolder<T>> Bulk { get; set; }
    }
}
