using Microsoft.AspNetCore.Mvc;
using Pantree.Data.Models.Contracts;
using Pantree.Services;

namespace Pantree.Store.Controllers
{
    public class ScanController : BaseController<ScanController>
    {
        public IActionResult LoadForm()
        {
            return PartialView("_ScanningForm");
        }

        public IActionResult Code(string Code)
        {
            return Process(Code);
        }

        public IActionResult Image(IFormFile Image)
        {
            var service = new ScanningService(AppConfig.ConnectionString);
            var code = service.ProcessBarcode(Image);

            return Process(code, service);
        }

        private IActionResult Process(string code)
        {
            return Process(code, new ScanningService(AppConfig.ConnectionString));
        }

        private IActionResult Process(string code, ScanningService service)
        {
            if (string.IsNullOrEmpty(code))
            {
                return PartialView("_ScanError");
            }
            else
            {
                if (User.UserDetails == null || !User.UserDetails.Locations.Any())
                {
                    var product = service.LookUpProduct<ProductReadOnly>(code);
                    product ??= new ProductReadOnly() { IsNull = true, ProductCode = code };
                    return PartialView("_ReadOnly", product);
                }

                var item = service.LookUpItem(code, User.UserID);

                var locations = LookupService.Locations(User.UserDetails.Locations, out int? locationID);
                var stores = LookupService.Stores(User.UserDetails.Locations.First(i => i.LocationID == locationID).Stores);

                if (item == null)
                {
                    var product = service.LookUpProduct<Product>(code);
                    if (product == null)
                    {
                        // Product can't be found via API, add new to database
                        var productAdd = new ProductAdd(code, locations, stores, User.UserDetails);
                        return PartialView("_NewProductForm", productAdd);
                    }
                    else
                    {
                        // New product, add details then save and add to stores
                        var itemAdd = new ItemAdd(product, locations, stores, User.UserDetails);
                        return PartialView("_NewItemForm", itemAdd);
                    }
                }
                else
                {
                    // Existing item, associated details, just save to store
                    var storedItemSave = new StoredItemAdd(item, locations, stores, User.UserDetails);
                    return PartialView("_SaveItemForm", storedItemSave);
                }
            }
        }

        [HttpPost]
        public IActionResult AddItem(ItemSave item)
        {
            var scanningService = new ScanningService(AppConfig.ConnectionString);
            scanningService.AddItem(item, User.UserID);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult AddProduct(ProductSave product)
        {
            var scanningService = new ScanningService(AppConfig.ConnectionString);
            scanningService.AddProduct(product, User.UserID);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult SaveItem(StoredItemSave item)
        {
            var scanningService = new ScanningService(AppConfig.ConnectionString);
            scanningService.SaveItem(item);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult StoreSelect(int locationID)
        {
            var model = new StoreSelect();

            var stores = User.UserDetails.Locations.Find(i => i.LocationID == locationID).Stores;
            model.Stores = LookupService.Stores(stores);
            model.ElementID = "DynamicStoreID";

            return PartialView("_StoreSelect", model);
        }
    }
}
