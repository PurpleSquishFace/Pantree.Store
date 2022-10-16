using Dapper;
using Mapster;
using MySql.Data.MySqlClient;
using Pantree.Data.Models;
using Pantree.Data.Models.Contracts;
using Pantree.Data.Models.Database;

namespace Pantree.Data.Access
{
    /// <summary>
    /// Methods to handle database access and information
    /// </summary>
    public class DatabaseAccess : DatabaseOperations, IDataAccess_User, IDataAccess_Image, IDataAccess_Scanning, IDataAccess_Friend, IDataAccess_Storage
    {
        public DatabaseAccess(string connectionString) : base(connectionString)
        {
        }

        #region User

        public int CreateUser(IDatabaseTable user)
        {
            return InsertSingle<tbl_Users>(user);
        }

        public T GetUser<T>(string username) where T : class
        {
            return ExecuteQuery<T>("SELECT * FROM tbl_Users WHERE Username = @username;", new { username });
        }

        public PantreeUser GetUser(string userName)
        {
            PantreeUser user;

            var sql = "SELECT * FROM tbl_Users T1 " +
                "LEFT JOIN tbl_ProfileImages T2 ON T1.UserID = T2.UserID " +
                "WHERE T1.UserName = @userName;";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                user = connection.Query<PantreeUser, ProfileImageView, PantreeUser>(sql,
                    (user, image) =>
                    {
                        user.ProfileImage = image;
                        return user;
                    },
                    splitOn: "ProfileImageID",
                    param: new { userName }).FirstOrDefault();
            }

            user = PopulateStorage(user);
            user = PopulateFriends(user);

            return user;
        }

        public PantreeUser GetUser(int userID)
        {
            PantreeUser user;
            var sql = "SELECT * FROM tbl_Users T1 " +
                "LEFT JOIN tbl_ProfileImages T2 ON T1.UserID = T2.UserID " +
                "WHERE T1.UserID = @userID";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                user = connection.Query<PantreeUser, ProfileImageView, PantreeUser>(sql,
                    (user, image) =>
                    {
                        user.ProfileImage = image;
                        return user;
                    },
                    splitOn: "ProfileImageID",
                    param: new { userID }).FirstOrDefault();
            }

            user = PopulateStorage(user);
            user = PopulateFriends(user);

            return user;
        }

        public PantreeUser PopulateStorage(PantreeUser user)
        {
            var sql = "SELECT * FROM tbl_Locations T1 LEFT JOIN tbl_Stores T2 ON T1.LocationID = T2.LocationID WHERE T1.UserID = @userID " +
            "UNION " +
            "SELECT T2.*, T3.* FROM tbl_SharedLocations T1 INNER JOIN tbl_Locations T2 ON T1.LocationID = T2.LocationID LEFT JOIN tbl_Stores T3 ON T2.LocationID = T3.LocationID WHERE T1.UserID = @userID;";

            var lookup = new Dictionary<int, LocationView>();
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Query<LocationView, StoreView, LocationView>(sql,
                (l, s) =>
                {
                    LocationView location;
                    if (!lookup.TryGetValue(l.LocationID, out location))
                        lookup.Add(l.LocationID, location = l);
                    if (location.Stores == null & s != null)
                        location.Stores = new List<StoreView>();
                    if (s != null)
                        location.Stores.Add(s);
                    return location;
                },
                new { userID = user.UserID },
                splitOn: "StoreID").AsQueryable();
            }

            user.Locations = lookup.Values.ToList();
            user = PopulateLocationUsers(user);
            return user;
        }

        public PantreeUser PopulateFriends(PantreeUser user)
        {
            user.Friends = GetFriends(user.UserID);
            return user;
        }

        public PantreeUser PopulateLocationUsers(PantreeUser user)
        {
            var list = new List<SharedUserView>();
            var sql = "SELECT T1.LocationID, T2.*, T3.* " +
                      "FROM tbl_Locations T1 " +
                      "INNER JOIN tbl_Users T2 ON T1.UserID = T2.UserID " +
                      "LEFT JOIN tbl_ProfileImages T3 ON T2.UserID = T3.UserID " +
                      "UNION " +
                      "SELECT T1.LocationID, T3.*, T4.* FROM tbl_Locations T1 " +
                      "LEFT JOIN tbl_SharedLocations T2 ON T1.LocationID = T2.LocationID " +
                      "INNER JOIN tbl_Users T3 ON T2.UserID = T3.UserID " +
                      "LEFT JOIN tbl_ProfileImages T4 ON T3.UserID = T4.UserID; ";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                list = connection.Query<SharedUserView, ProfileImageView, SharedUserView>(sql,
                    (u, i) =>
                    {
                        u.User_ProfileImage = i;
                        return u;
                    },
                    splitOn: "ProfileImageID").ToList();
            }

            user.Locations.ForEach(i => i.SharedUsers = list.Where(j => j.LocationID == i.LocationID).ToList());
            return user;
        }

        public void UpdateUserDetails(int userID, string displayName)
        {
            string sql = "UPDATE tbl_Users SET DisplayName = @displayName WHERE UserID = @userID;";
            ExecuteQuery(sql, new { userID, displayName });
        }

        #endregion

        #region Image

        public int CreateProfileImage(IDatabaseTable profileImage)
        {
            return InsertSingle<tbl_ProfileImages>(profileImage);
        }

        public void UpdateProfileImage(int profileImageID, byte[] profileImage, string alternativeText)
        {
            var sql = "UPDATE tbl_ProfileImages SET ProfileImage = @profileImage, AlternativeText = @alternativeText WHERE ProfileImageID = @profileImageID;";
            ExecuteQuery(sql, new { profileImageID, profileImage, alternativeText });
        }

        public bool RemoveProfileImage(IDatabaseTable profileImage)
        {
            return DeleteSingle<tbl_ProfileImages>(profileImage);
        }

        #endregion

        #region ScanningService

        public int AddProduct(IDatabaseTable product)
        {
            return InsertSingle<tbl_Products>(product);
        }

        public int AddItem(IDatabaseTable item)
        {
            return InsertSingle<tbl_Items>(item);
        }

        public bool UpdateItem(IDatabaseTable item)
        {
            return UpdateSingle<tbl_Items>(item);
        }

        public T GetProduct<T>(int productID)
        {
            return GetSingle<tbl_Products>(productID).Adapt<T>();
        }

        public T GetProduct<T>(string productCode) where T : class
        {
            var sql = "SELECT * FROM tbl_Products WHERE ProductCode = @productCode";
            return ExecuteQuery<T>(sql, new { productCode });
        }

        public T GetItem<T>(string productCode, int userID) where T : class
        {
            var sql = "SELECT * FROM uvw_Items WHERE ProductCode = @productCode AND UserID = @userID;";
            return ExecuteQuery<T>(sql, new { productCode, userID });
        }

        public T GetItem<T>(int itemID) where T : class
        {
            var sql = "SELECT T1.ItemID, T1.UserID, T1.ProductID, T2.ProductCode, " +
                    "COALESCE(T1.ProductName, T2.ProductName) AS ProductName, " +
                    "COALESCE(T1.IngredientList, T2.IngredientList) AS IngredientList, " +
                    "COALESCE(T1.ImageUrl, T2.ImageUrl) AS ImageUrl, " +
                    "T1.Notes, " +
                    "T2.LookupsSinceScan, " +
                    "T1.CreatedDate " +
                    "FROM tbl_Items T1 INNER JOIN tbl_Products T2 ON T1.ProductID = T2.ProductID " +
                    "WHERE T1.ItemID = @itemID;";
            return ExecuteQuery<T>(sql, new { itemID });
        }

        public T GetItem_Record<T>(int itemID)
        {
            return GetSingle<tbl_Items>(itemID).Adapt<T>();
        }

        public void StoreItem(tbl_StoredItems storedItem)
        {
            var sql = "SELECT * FROM tbl_StoredItems WHERE StoreID = @storeID AND ItemID = @itemID";
            var current = ExecuteQuery<tbl_StoredItems>(sql, new { storeID = storedItem.StoreID, itemID = storedItem.ItemID });

            if (current == null)
                InsertSingle<tbl_StoredItems>(storedItem);
            else
            {
                storedItem.Quantity += current.Quantity;
                UpdateSingle<tbl_StoredItems>(storedItem);
            }
        }

        #endregion

        #region Friend

        public List<FriendView> GetFriends(int userID)
        {
            var list = new List<FriendView>();

            var sql = "SELECT T1.*, " +
                    "CASE WHEN T1.UserID_Requester = @userID THEN T3.UserID " +
                    "WHEN T1.UserID_Addressee = @userID THEN T2.UserID END AS Friend_UserID, " +
                    "CASE WHEN T1.UserID_Requester = @userID THEN T3.UserName " +
                    "WHEN T1.UserID_Addressee = @userID THEN T2.UserName END AS Friend_UserName, " +
                    "CASE WHEN T1.UserID_Requester = @userID THEN T3.DisplayName " +
                    "WHEN T1.UserID_Addressee = @userID THEN T2.DisplayName END AS Friend_DisplayName, " +
                    "T1.FriendStatusID AS FriendStatus, " +
                    "CASE WHEN T1.UserID_Requester = @userID THEN T5.ProfileImageID " +
                    "WHEN T1.UserID_Addressee = @userID THEN T4.ProfileImageID END AS ProfileImageID, " +
                    "CASE WHEN T1.UserID_Requester = @userID THEN T5.ProfileImage " +
                    "WHEN T1.UserID_Addressee = @userID THEN T4.ProfileImage END AS ProfileImage, " +
                    "CASE WHEN T1.UserID_Requester = @userID THEN T5.AlternativeText " +
                    "WHEN T1.UserID_Addressee = @userID THEN T4.AlternativeText END AS AlternativeText " +
                    "FROM tbl_UserFriends T1 " +
                    "INNER JOIN tbl_Users T2 ON T1.UserID_Requester = T2.UserID " +
                    "INNER JOIN tbl_Users T3 ON T1.UserID_Addressee = T3.UserID " +
                    "LEFT JOIN tbl_ProfileImages T4 ON T1.UserID_Requester = T4.UserID " +
                    "LEFT JOIN tbl_ProfileImages T5 ON T1.UserID_Addressee = T5.UserID " +
                    "WHERE(UserID_Requester = @userID OR UserID_Addressee = @userID);";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                list = connection.Query<FriendView, ProfileImageView, FriendView>(sql,
                    (user, image) =>
                    {
                        user.Friend_ProfileImage = image;
                        return user;
                    },
                    splitOn: "ProfileImageID",
                    param: new { userID }).ToList();
            }

            return list;
        }

        public List<UserSearchResult> SearchUsers(string searchQuery, int userID)
        {
            var list = new List<UserSearchResult>();

            if (string.IsNullOrEmpty(searchQuery))
            {
                return list;
            }

            var sql = "SELECT T1.*, " +
                    "CASE WHEN T1.UserID = @userID THEN 5 ELSE COALESCE(T3.FriendStatusID, T4.FriendStatusID, 6) END AS 'Status', " +
                    "T2.ProfileImageID, T2.ProfileImage, T2.AlternativeText " +
                    "FROM tbl_Users T1 " +
                    "LEFT JOIN tbl_ProfileImages T2 ON T1.UserID = T2.UserID " +
                    "LEFT JOIN tbl_UserFriends T3 ON T1.UserID = T3.UserID_Requester AND T3.UserID_Addressee = @userID " +
                    "LEFT JOIN tbl_UserFriends T4 ON T1.UserID = T4.UserID_Addressee AND T4.UserID_Requester = @userID ";

            sql += searchQuery.Length > 5 ? "WHERE Username LIKE CONCAT('%', @searchQuery, '%');" : "WHERE UserName = @searchQuery;";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                list = connection.Query<UserSearchResult, ProfileImageView, UserSearchResult>(sql,
                    (user, image) =>
                    {
                        if (image != null)
                        {
                            user.User_ProfileImage = image;
                        }
                        return user;
                    },
                    splitOn: "ProfileImageID",
                    param: new { searchQuery, userID }).ToList();
            }

            return list;
        }

        public int AddUserFriend(IDatabaseTable friendUser)
        {
            return InsertSingle<tbl_UserFriends>(friendUser);
        }

        public T GetUserFriend<T>(int userFriendID)
        {
            return GetSingle<tbl_UserFriends>(userFriendID).Adapt<T>();
        }

        public bool UpdateFriendStatus(IDatabaseTable friendUser)
        {
            return UpdateSingle<tbl_UserFriends>(friendUser);
        }

        public bool RemoveUserFriend(IDatabaseTable friendUser)
        {
            return DeleteSingle<tbl_UserFriends>(friendUser);
        }

        #endregion

        #region Storage

        public int AddLocation(IDatabaseTable location)
        {
            return InsertSingle<tbl_Locations>(location);
        }

        public int AddStore(IDatabaseTable store)
        {
            return InsertSingle<tbl_Stores>(store);
        }

        public T GetLocation<T>(int locationID)
        {
            return GetSingle<tbl_Locations>(locationID).Adapt<T>();
        }

        public void UpdateLocationName(int locationID, string locationName)
        {
            string sql = "UPDATE tbl_Locations SET LocationName = @locationName WHERE LocationID = @locationID;";
            ExecuteQuery(sql, new { locationID, locationName });
        }

        public void UpdateLocationDescription(int locationID, string description)
        {
            string sql = "UPDATE tbl_Locations SET Description = @description WHERE LocationID = @locationID;";
            ExecuteQuery(sql, new { locationID, description });
        }

        public void UpdateStoreName(int storeID, string storeName)
        {
            string sql = "UPDATE tbl_Stores SET StoreName = @storeName WHERE StoreID = @storeID;";
            ExecuteQuery(sql, new { storeID, storeName });
        }

        public void UpdateStoreDescription(int storeID, string description)
        {
            string sql = "UPDATE tbl_Stores SET Description = @description WHERE StoreID = @storeID;";
            ExecuteQuery(sql, new { storeID, description });
        }

        public bool DeleteLocation(int locationID, int userID)
        {
            var sql = "SELECT * FROM tbl_Locations WHERE LocationID = @locationID AND UserID = @userID;";
            var record = ExecuteQuery<tbl_Locations>(sql, new { locationID, userID });

            if (record != null) return DeleteSingle<tbl_Locations>(record);

            return false;
        }

        public void DeleteStores(int locationID, int userID)
        {
            var sql = "SELECT * FROM tbl_Stores WHERE LocationID = @locationID AND UserID = @userID";
            var records = ExecuteQueryList<tbl_Stores>(sql, new { locationID, userID });
            DeleteMultiple(records);
        }

        public bool DeleteStore(int storeID, int userID)
        {
            var sql = "SELECT * FROM tbl_Stores WHERE StoreID = @storeID AND UserID = @userID;";
            var record = ExecuteQuery<tbl_Stores>(sql, new { storeID, userID });

            if (record != null) return DeleteSingle<tbl_Stores>(record);

            return false;
        }

        public T GetStoredItem<T>(int itemID, int storeID)
        {
            var sql = "SELECT * FROM uvw_Items_Store WHERE ItemID = @itemID AND StoreID = @storeID;";
            return ExecuteQuery<T>(sql, new { itemID, storeID });
        }

        public List<T> GetStoredItems<T>(int storeID)
        {
            var sql = "SELECT * FROM uvw_Items_Store WHERE StoreID = @storeID;";

            return ExecuteQueryList<T>(sql, new { storeID });
        }

        public bool UpdateItemQuantity(int itemID, int storeID, int quantity)
        {
            var item = GetStoredItem<tbl_StoredItems>(itemID, storeID);
            item.Quantity += quantity;
            item.Quantity = item.Quantity < 0 ? 0 : item.Quantity;

            return UpdateSingle<tbl_StoredItems>(item);
        }

        public List<T> SearchStorage<T>(string searchQuery, int userID)
        {
            var sql = "SELECT * FROM uvw_Items_Store_Search WHERE ProductName LIKE CONCAT('%', @searchQuery, '%') AND UserID = @userID;";
            return ExecuteQueryList<T>(sql, new { searchQuery, userID });
        }

        public bool DeleteItem(int itemID, int storeID)
        {
            var item = GetStoredItem<tbl_StoredItems>(itemID, storeID);
            return DeleteSingle<tbl_StoredItems>(item);
        }

        public T GetStore<T>(int storeID)
        {
            return GetSingle<tbl_Stores>(storeID).Adapt<T>();
        }

        public int AddSharedLocation(IDatabaseTable share)
        {
            return InsertSingle<tbl_SharedLocations>(share);
        }

        public bool DeleteSharedLocation(int userID, int locationID)
        {
            var sql = "SELECT * FROM tbl_SharedLocations WHERE UserID = @userID AND LocationID = @locationID;";
            var record = ExecuteQuery<tbl_SharedLocations>(sql, new { userID, locationID });
            if (record != null) return DeleteSingle<tbl_SharedLocations>(record);

            return false;
        }

        public void DeleteSharedLocation(int LocationID)
        {
            var sql = "SELECT * FROM tbl_SharedLocations WHERE LocationID = @locationID;";
            var records = ExecuteQueryList<tbl_SharedLocations>(sql, new { LocationID });
            DeleteMultiple(records);
        }

        #endregion
    }
}