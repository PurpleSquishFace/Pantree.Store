using Pantree.Data.Models;
using Pantree.Data.Models.Database;

namespace Pantree.Data.Access
{
    public interface IDataAccess_Scanning
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        int AddProduct(IDatabaseTable product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int AddItem(IDatabaseTable item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool UpdateItem(IDatabaseTable item);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="productID"></param>
        /// <returns></returns>
        T GetProduct<T>(int productID);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="productCode"></param>
        /// <returns></returns>
        T GetProduct<T>(string productCode) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="productCode"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        T GetItem<T>(string productCode, int userID) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemID"></param>
        /// <returns></returns>
        T GetItem<T>(int itemID) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemID"></param>
        /// <returns></returns>
        T GetItem_Record<T>(int itemID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storedItem"></param>
        void StoreItem(tbl_StoredItems storedItem);
    }
}
