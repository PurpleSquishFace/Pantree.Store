using Microsoft.AspNetCore.Http;
using Pantree.Data.Models.Contracts;
using Pantree.Data.Models.Database;
using Pantree.Scanning;

namespace Pantree.Services
{
    /// <summary>
    /// A service class with methods used with scanning processes.
    /// </summary>
    public class ScanningService : PantreeService
    {
        /// <summary>
        /// Creates a new instance of the ScanningService class.
        /// </summary>
        /// <param name="dbConnectionString">The connection string to generate a database connection.</param>
        public ScanningService(string dbConnectionString) : base(dbConnectionString)
        {

        }

        /// <summary>
        /// Processes an uploaded image to extract the barcode value.
        /// </summary>
        /// <param name="imageFile">The uploaded image.</param>
        /// <returns>The extracted barcode value, or an empty string if one can't be read.</returns>
        public string ProcessBarcode(IFormFile imageFile)
        {
            var barcode = new BarcodeScanner(imageFile);

            if (barcode.ReadBarcode())
                return barcode.OutputCode;
            else
                return string.Empty;
        }

        /// <summary>
        /// Looks up an item in the database, if the current user has already added it.
        /// </summary>
        /// <param name="code">The product code, commonly a barcode value.</param>
        /// <param name="userID">The User ID of the current user.</param>
        /// <returns>The item, if found, with it's related properties as save by the user.</returns>
        public Item LookUpItem(string code, int userID)
        {
            return db.GetItem<Item>(code, userID);
        }

        /// <summary>
        /// Looks up a product in the database, if one cannot be found then searches the product using the API.
        /// </summary>
        /// <typeparam name="T">The type to return the product as.</typeparam>
        /// <param name="code">The product code, commonly a barcode value.</param>
        /// <returns>The product, if found.</returns>
        public T LookUpProduct<T>(string code) where T : class
        {
            var product = db.GetProduct<T>(code);

            if (product == null)
                return LookUpProductAPI<T>(code);
            else
                return product;
        }

        /// <summary>
        /// Looks up a product via the API, and if found saves the data to the database and returns the result.
        /// </summary>
        /// <typeparam name="T">The type to return the product as.</typeparam>
        /// <param name="code">The product code, commonly a barcode value.</param>
        /// <returns>The product, if foumd.</returns>
        private T LookUpProductAPI<T>(string code) where T : class
        {
            var api = new FoodFactsAPI(code);

            if (api.CallAPI())
            {
                var product = new tbl_Products
                {
                    ProductCode = api.Product.ProductCode,
                    ProductName = api.Product.ProductName,
                    IngredientList = api.Product.IngredientList,
                    ImageUrl = api.Product.ImageURL,
                    CreatedDate = DateTime.Now
                };

                db.AddProduct(product);
            }

            return db.GetProduct<T>(code);
        }

        /// <summary>
        /// Saves an item to the database, saving the user specific changes to the property, and saves where the item is stored as well.
        /// </summary>
        /// <param name="itemSave">The item to be saved.</param>
        /// <param name="userID">The User ID of the current user.</param>
        /// <returns>The added item.</returns>
        public Item AddItem(ItemSave itemSave, int userID)
        {
            var item = new tbl_Items
            {
                UserID = userID,
                ProductID = itemSave.ProductID,
                ProductName = string.IsNullOrEmpty(itemSave.ProductName) ? null : itemSave.ProductName,
                IngredientList = string.IsNullOrEmpty(itemSave.IngredientList) ? null : itemSave.IngredientList,
                ImageUrl = string.IsNullOrEmpty(itemSave.ImageUrl) ? null : itemSave.ImageUrl,
                Notes = itemSave.Notes,
                CreatedDate = DateTime.Now
            };

            var itemID = db.AddItem(item);

            UpdateStoredItem(itemSave.StoreID, itemID, itemSave.Quantity);

            return db.GetItem<Item>(itemID);
        }
        
        /// <summary>
        /// Saves a product to the database, then as a user specific item, then finally where the item is stored as well.
        /// </summary>
        /// <param name="productSave">The product to be saved.</param>
        /// <param name="userID">The User ID of the current user.</param>
        /// <returns>The added product, as an item.</returns>
        public Item AddProduct(ProductSave productSave, int userID)
        {
            var product = new tbl_Products
            {
                ProductCode = productSave.ProductCode,
                ProductName = productSave.ProductName,
                IngredientList = productSave.IngredientList,
                ImageUrl = "/Images/Placeholder.jpg",
                LookupsSinceScan = 0,
                CreatedDate = DateTime.Now
            };
            var productID = db.AddProduct(product);

            var item = new tbl_Items
            {
                UserID = userID,
                ProductID = productID,
                Notes = productSave.Notes,
                CreatedDate = DateTime.Now
            };
            var itemID = db.AddItem(item);

            UpdateStoredItem(productSave.StoreID, itemID, productSave.Quantity);

            return db.GetItem<Item>(itemID);
        }

        /// <summary>
        /// Updates an item in the database, saving user specific product details, as well as saving where the item is stored.
        /// </summary>
        /// <param name="storedItemSave">The updated item details.</param>
        /// <returns>The saved Item.</returns>
        public Item SaveItem(StoredItemSave storedItemSave)
        {
            var item = db.GetItem_Record<tbl_Items>(storedItemSave.ItemID);
            item.ProductName = storedItemSave.ProductName;
            item.IngredientList = storedItemSave.IngredientList;
            item.Notes = storedItemSave.Notes;
            db.UpdateItem(item);

            UpdateStoredItem(storedItemSave.StoreID, storedItemSave.ItemID, storedItemSave.Quantity);

            return db.GetItem<Item>(storedItemSave.ItemID);
        }

        /// <summary>
        /// Saves where an item is saved, and the quantity of the item.
        /// </summary>
        /// <param name="storeID">The Store ID of the store where the item is being kept.</param>
        /// <param name="itemID">The Item ID of the item being kept.</param>
        /// <param name="quantity">The number of items being kept in the store.</param>
        public void UpdateStoredItem(int storeID, int itemID, int quantity)
        {
            var storedItem = new tbl_StoredItems
            {
                StoreID = storeID,
                ItemID = itemID,
                Quantity = quantity
            };

            db.StoreItem(storedItem);
        }
    }
}