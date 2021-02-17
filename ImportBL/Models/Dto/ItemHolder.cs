namespace ImportBL.Models.Dto
{
    public class ItemHolder<T> where T: Item
    {
        public T Fields { get; set; }
    }
}
