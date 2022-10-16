using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pantree.Data.Models.Contracts
{
    public class StorageMain
    {
        public List<LocationView> Locations { get; set; }
        public LocationAdd LocationAdd { get; set; }
        public bool AddLocation { get; set; }
        public bool Search { get; set; }
        public int? SelectedLocationID { get; private set; }
        public LocationView SelectedLocation { get; set; }
        public int? SelectedStoreID { get; set; }
        public StoreView SelectedStore
        {
            get
            {
                var store = Locations.Find(i => i.LocationID == SelectedLocationID).Stores.Find(i => i.StoreID == SelectedStoreID);
                store.Items = Items;
                return store;
            }
        }
        public List<StoredItem> Items { get; set; }

        public StorageSearchMain StorageSearch { get; set; }

        public StorageMain(int userID, List<LocationView> locations, SelectList friends, int? selectedLocationID = null)
        {
            SelectedLocationID = selectedLocationID;
            Locations = locations ?? new List<LocationView>();
            SelectedLocation = Locations.Find(i => i.LocationID == SelectedLocationID) ?? new LocationView();
            SelectedLocation.LocationShare = new LocationShare(userID, SelectedLocation?.SharedUsers, friends)
            {
                LocationID = SelectedLocationID ?? 0,
            };

            LocationAdd = new LocationAdd();
            StorageSearch = new StorageSearchMain();
        }
    }
}