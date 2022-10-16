namespace Pantree.Data.Models.Contracts
{
    public class ProductSave
    {
        public string ProductCode { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public string IngredientList { get; set; }
        public string Notes { get; set; }
        public int LocationID { get; set; }
        public int StoreID { get; set; }
        public int Quantity { get; set; }
    }
}