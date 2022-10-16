namespace Pantree.Data.Models.Contracts
{
    public class StoredItemSave
    {
        public int ItemID { get; set; }
        public string ProductName { get; set; }
        public string IngredientList { get; set; }
        public string Notes { get; set; }
        public int LocationID { get; set; }
        public int StoreID { get; set; }
        public int Quantity { get; set; }
    }
}