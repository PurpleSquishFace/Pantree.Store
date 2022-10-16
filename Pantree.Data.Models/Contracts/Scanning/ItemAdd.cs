using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pantree.Data.Models.Contracts
{
    public class ItemAdd : BaseSave
    {
        public int ProductID { get; set; }

        public ItemAdd(Product product, SelectList locations, SelectList stores, PantreeUser user)
        {
            this.ProductID = product.ProductID;
            this.ProductCode = product.ProductCode;
            this.ImageUrl = product.ImageUrl;
            this.ProductName = product.ProductName;
            this.IngredientList = product.IngredientList;
            this.Locations = locations;
            this.Stores = stores;

            //this.Locations = LookupService.Locations(user.Locations);
            //this.Stores = LookupService.Stores(user.Locations.First().Stores);
        }
    }
}