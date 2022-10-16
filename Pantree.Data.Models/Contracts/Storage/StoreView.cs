namespace Pantree.Data.Models.Contracts
{
    public class StoreView
    {
        public int StoreID { get; set; }
        public int LocationID { get; set; }
        public string StoreName { get; set; }
        public string Description { get; set; }
        public bool OwnStore { get; set; }
        public List<StoredItem> Items { get; set; }
        public int UserID { get; set; }
    }
}