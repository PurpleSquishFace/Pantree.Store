namespace Pantree.Data.Models.Contracts
{
    public class Item : BaseItem
    {
        public int ItemID { get; set; }
        public int ProductID { get; set; }
        public string Notes { get; set; }
        public int Quantity { get; set; }
        public int StoredQuantity { get; set; }
    }
}