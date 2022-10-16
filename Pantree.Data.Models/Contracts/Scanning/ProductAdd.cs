using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pantree.Data.Models.Contracts
{
    public class ProductAdd : BaseSave
    {
        public IFormFile Image { get; set; }
        public ProductAdd(string code, SelectList locations, SelectList stores, PantreeUser user)
        {
            this.ProductCode = code;
            this.Locations = locations;
            this.Stores = stores;

            //this.Locations = LookupService.Locations(user.Locations);
            //this.Stores = LookupService.Stores(user.Locations.First().Stores);
        }
    }
}