using System;

namespace ImportBL.Models
{
    public abstract class Item : IEquatable<Item>
    {
        public string Id { get; set; }

        public abstract bool Equals(Item other);
    }
}
