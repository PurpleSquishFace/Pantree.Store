using Pantree.Data.Models;

namespace Pantree.Data.Access
{
    public interface IDataAccess_Storage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public int AddLocation(IDatabaseTable location);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public int AddStore(IDatabaseTable store);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="locationID"></param>
        /// <returns></returns>
        public T GetLocation<T>(int locationID);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationID"></param>
        /// <param name="locationName"></param>
        public void UpdateLocationName(int locationID, string locationName);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationID"></param>
        /// <param name="description"></param>
        public void UpdateLocationDescription(int locationID, string description);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="storeName"></param>
        public void UpdateStoreName(int storeID, string storeName);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="description"></param>
        public void UpdateStoreDescription(int storeID, string description);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool DeleteLocation(int locationID, int userID);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationID"></param>
        /// <param name="userID"></param>
        public void DeleteStores(int locationID, int userID);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool DeleteStore(int storeID, int userID);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemID"></param>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public T GetStoredItem<T>(int itemID, int storeID);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public List<T> GetStoredItems<T>(int storeID);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="storeID"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool UpdateItemQuantity(int itemID, int storeID, int quantity);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchQuery"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<T> SearchStorage<T>(string searchQuery, int userID);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public bool DeleteItem(int itemID, int storeID);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public T GetStore<T>(int storeID);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        public int AddSharedLocation(IDatabaseTable share);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="locationID"></param>
        /// <returns></returns>
        public bool DeleteSharedLocation(int userID, int locationID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LocationID"></param>
        public void DeleteSharedLocation(int LocationID);
    }
}