namespace Pantree.Data.Models.Contracts
{
    public class StoredItem : BaseItem
    {
        public int ItemID { get; set; }
        public int ProductID { get; set; }
        public int StoreID { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public int LookupsSinceScan { get; set; }
    }
}