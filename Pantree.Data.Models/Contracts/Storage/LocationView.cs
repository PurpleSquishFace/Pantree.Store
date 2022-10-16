namespace Pantree.Data.Models.Contracts
{
    public class LocationView
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public List<StoreView> Stores { get; set; } = new List<StoreView>();
        public int UserID { get; set; }
        public List<SharedUserView> SharedUsers { get; set; }

        public StoreAdd StoreAdd => new StoreAdd() { LocationID = this.LocationID };
        public LocationShare LocationShare { get; set; }
        public LocationDelete LocationDelete => new LocationDelete() { LocationID = this.LocationID, LocationName = this.LocationName };
    }
}