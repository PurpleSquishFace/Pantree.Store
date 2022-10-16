using System.ComponentModel;

namespace Pantree.Data.Models.Contracts
{
    public class BaseItem
    {
        [DisplayName("Code")]
        public string ProductCode { get; set; }

        [DisplayName("Name")]
        public string ProductName { get; set; }

        [DisplayName("Ingredients")]
        public string IngredientList { get; set; }

        public string ImageUrl { get; set; }
    }
}