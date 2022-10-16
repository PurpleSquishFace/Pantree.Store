using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pantree.Data.Models.Contracts
{
    public class StoredItemAdd : BaseSave
    {
        public int ItemID { get; set; }
        public string Image { get; set; }
        public int StoredQuantity { get; set; }

        public StoredItemAdd(Item item, SelectList locations, SelectList stores, PantreeUser user)
        {
            this.ItemID = item.ItemID;
            this.ProductCode = item.ProductCode;
            this.ImageUrl = item.ImageUrl;
            this.ProductName = item.ProductName;
            this.Notes = item.Notes;
            this.IngredientList = item.IngredientList;
            this.StoredQuantity = item.StoredQuantity;
            this.Locations = locations;
            this.Stores = stores;

            //this.Locations = LookupService.Locations(user.Locations);
            //this.Stores = LookupService.Stores(user.Locations.First().Stores);
        }
    }
}