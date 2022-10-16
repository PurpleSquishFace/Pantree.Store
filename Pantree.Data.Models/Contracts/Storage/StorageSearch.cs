namespace Pantree.Data.Models.Contracts
{
    public class StorageSearch
    {
        public int ItemID { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantity { get; set; }
        public List<StorageSearchResult> Items { get; set; }
    }
}