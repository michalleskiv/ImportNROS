using System;

namespace ImportBL.Models
{
    public abstract class Item : IComparable<Item>
    {
        public string Id { get; set; }

        public abstract int CompareTo(Item other);
    }
}
