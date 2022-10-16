namespace Pantree.Data.Models.Contracts
{
    public class StorageSearchResult
    {
        public int ItemID { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public int LocationID { get; set; }
        public int StoreID { get; set; }
        public string LocationName { get; set; }
        public string StoreName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string IngredientList { get; set; }
        public string ImageUrl { get; set; }
        public string Notes { get; set; }
        public int LookupsSinceScan { get; set; }
        public int Quantity { get; set; }
        public int TotalQuantity { get; set; }
    }
}