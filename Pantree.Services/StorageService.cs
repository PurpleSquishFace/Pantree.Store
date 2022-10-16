using Pantree.Data.Access;
using Pantree.Data.Models.Contracts;
using Pantree.Data.Models.Database;

namespace Pantree.Services
{
    /// <summary>
    /// A service class with methods providing functionality related to locations, stores, and the items within them.
    /// </summary>
    public class StorageService : PantreeService
    {
        /// <summary>
        /// Creates a new instance of the StorageService class.
        /// </summary>
        /// <param name="dbConnectionString">The connection string to generate a database connection.</param>
        public StorageService(string dbConnectionString) : base(dbConnectionString)
        {
        }

        /// <summary>
        /// Creates a new location in the database.
        /// </summary>
        /// <param name="location">The details of the new location.</param>
        /// <param name="userID">The User ID of the user the location is attributed to.</param>
        /// <returns>The Location ID of the newly created record.</returns>
        public int AddLocation(LocationAdd location, int userID)
        {
            var newLocation = new tbl_Locations
            {
                LocationName = location.LocationName,
                Description = location.LocationDescription,
                UserID = userID,
                CreatedDate = DateTime.Now
            };

            return db.AddLocation(newLocation);
        }

        /// <summary>
        /// Creates a new store in the database.
        /// </summary>
        /// <param name="store">The details of the new store.</param>
        /// <param name="userID">The User ID of the user the store is attributed to.</param>
        /// <returns>The Store ID of the newly created record.</returns>
        public int AddStore(StoreAdd store, int userID)
        {
            var newStore = new tbl_Stores
            {
                LocationID = store.LocationID,
                StoreName = store.StoreName,
                Description = store.StoreDescription,
                UserID = userID,
                CreatedDate = DateTime.Now
            };

            return db.AddStore(newStore);
        }

        /// <summary>
        /// Get location details for a front end view.
        /// </summary>
        /// <param name="locationID">The Location ID of the location to be returned.</param>
        /// <returns>The location if found.</returns>
        public LocationView GetLocation(int locationID)
        {
            return db.GetLocation<LocationView>(locationID);
        }

        /// <summary>
        /// Update the name of a location in the database.
        /// </summary>
        /// <param name="edit">Object containing the Location ID and updated location name.</param>
        public void UpdateLocationName(LocationNameEdit edit)
        {
            db.UpdateLocationName(edit.LocationID, edit.LocationName);
        }

        /// <summary>
        /// Update the description property of a location in the database.
        /// </summary>
        /// <param name="edit">Object containing the Location ID and updated location description.</param>
        public void UpdateLocationDescription(LocationDescriptionEdit edit)
        {
            db.UpdateLocationDescription(edit.LocationID, edit.Description);
        }

        /// <summary>
        /// Update the name of a store in the database.
        /// </summary>
        /// <param name="edit">Object containing the Store ID and updated store name.</param>
        public void UpdateStoreName(StoreNameEdit edit)
        {
            db.UpdateStoreName(edit.StoreID, edit.StoreName);
        }

        /// <summary>
        /// Update the description property of a store in the database.
        /// </summary>
        /// <param name="edit">Object containing the Store ID and updated location description.</param>
        public void UpdateStoreDescription(StoreDescriptionEdit edit)
        {
            db.UpdateStoreDescription(edit.StoreID, edit.Description);
        }

        /// <summary>
        /// Delete a location from the database, including any stores linked to the location.
        /// </summary>
        /// <param name="locationID">The Location ID of the location record to be deleted.</param>
        /// <param name="userID">The User ID of the user completing the delete action.</param>
        /// <returns>Whether the delete operation was successful.</returns>
        public bool DeleteLocation(int locationID, int userID)
        {
            db.DeleteStores(locationID, userID);
            db.DeleteSharedLocation(locationID);
            return db.DeleteLocation(locationID, userID);
        }

        /// <summary>
        /// Delete a store from the database.
        /// </summary>
        /// <param name="storeID">The Store ID of the store record to be deleted.</param>
        /// <param name="userID">The User ID of the user completing the delete action.</param>
        /// <returns>Whether the delete operation was successful.</returns>
        public bool DeleteStore(int storeID, int userID)
        {
            return db.DeleteStore(storeID, userID);
        }

        /// <summary>
        /// Get a list of items stored in a particular Store.
        /// </summary>
        /// <param name="storeID">The Store ID of the store the items are kept in.</param>
        /// <returns>The list of items, if any.</returns>
        public List<StoredItem> GetItems(int storeID)
        {
            return db.GetStoredItems<StoredItem>(storeID);
        }

        /// <summary>
        /// Updates database to record an increase in the quantity of items in the database.
        /// </summary>
        /// <param name="itemID">The Item ID of the item to update in the store.</param>
        /// <param name="storeID">The Store ID of the store where the item is kept.</param>
        /// <param name="quantity">The amount of items to increase by.</param>
        /// <returns>The details of the stored item in the store.</returns>
        public StoredItem AddItem(int itemID, int storeID, int quantity)
        {
            if (db.UpdateItemQuantity(itemID, storeID, quantity))
            {
                return db.GetStoredItem<StoredItem>(itemID, storeID);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Updates database to record a decrease in the quantity of items in the database.
        /// </summary>
        /// <param name="itemID">The Item ID of the item to update in the store.</param>
        /// <param name="storeID">The Store ID of the store where the item is kept.</param>
        /// <param name="quantity">The amount of items to decrease by.</param>
        /// <returns>The details of the stored item in the store.</returns>
        public StoredItem RemoveItem(int itemID, int storeID, int quantity)
        {
            if (db.UpdateItemQuantity(itemID, storeID, quantity *= -1))
            {
                return db.GetStoredItem<StoredItem>(itemID, storeID);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Updates database to completely remove an instance of a stored item, regardless of current quantity.
        /// </summary>
        /// <param name="itemID">The Item ID of the item to be deleted.</param>
        /// <param name="storeID">The Store ID of the store containing the item to be deleted.</param>
        /// <returns>Whether the deleted operation was successful.</returns>
        public bool DeleteItem(int itemID, int storeID)
        {
            return db.DeleteItem(itemID, storeID);
        }

        /// <summary>
        /// Searches the database for products stored in locations owned by the current user, where the product name contains the provided search query.<br/>
        /// When the result of the lookup is returned, duplicate products are condensed down, with storage data.
        /// </summary>
        /// <param name="searchQuery">The search query to lookup against stored product names.</param>
        /// <param name="userID">The User ID of the current user.</param>
        /// <returns>The search results as a list.</returns>
        public List<StorageSearch> Search(string searchQuery, int userID)
        {
            var results = db.SearchStorage<StorageSearchResult>(searchQuery, userID);
            var search = results.Select(i => new StorageSearch() { ItemID = i.ItemID, ProductName = i.ProductName, TotalQuantity = i.TotalQuantity }).ToList();
            search = search.GroupBy(i => i.ItemID).Select(g => g.First()).ToList();

            foreach (var item in search)
            {
                item.Items = results.Where(i => i.ItemID == item.ItemID).ToList();
            }

            return search;
        }

        /// <summary>
        /// Get store details for a front end view.
        /// </summary>
        /// <param name="storeID">The Store ID of the store to be returned.</param>
        /// <returns>The store if found.</returns>
        public StoreView GetStore(int storeID)
        {
            return db.GetStore<StoreView>(storeID);
        }

        /// <summary>
        /// Updates the database to record a user sharing a location with another user.
        /// </summary>
        /// <param name="locationShare">Object containing details to share the location, including the User ID of the user to be shared with, and the Location ID of the location.</param>
        /// <returns>The unique ID of the newly generated record.</returns>
        public int ShareLocation(LocationShareSubmit locationShare)
        {
            var newShare = new tbl_SharedLocations()
            {
                UserID = locationShare.UserID,
                LocationID = locationShare.LocationID
            };

            return db.AddSharedLocation(newShare);
        }

        /// <summary>
        /// Updates the database to remove the record detailing a location that is shared with another user.
        /// </summary>
        /// <param name="locationUnshare">Object containing details to remove the shared location.</param>
        /// <returns>Whether the delete operation was successful.</returns>
        public bool UnshareLocation(LocationUnshare locationUnshare)
        {
            return db.DeleteSharedLocation(locationUnshare.UserID, locationUnshare.LocationID);
        }
    }
}