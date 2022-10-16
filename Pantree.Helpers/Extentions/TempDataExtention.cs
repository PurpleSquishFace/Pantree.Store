using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Pantree.Helpers.Extentions
{
    /// <summary>
    /// Extension methods to handle objects with TempData.
    /// </summary>
    public static class TempDataExtensions
    {
        /// <summary>
        /// Sets a an object into TempData.
        /// </summary>
        /// <typeparam name="T">The type of the object being set.</typeparam>
        /// <param name="tempData">The current TempData object.</param>
        /// <param name="key">The unique key of the TempData item.</param>
        /// <param name="value">The object to set into TempData.</param>
        public static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// Gets an object from TempData.
        /// </summary>
        /// <typeparam name="T">The type of the object being retrieved.</typeparam>
        /// <param name="tempData">The current TempData object.</param>
        /// <param name="key">The unique key of the TempData item.</param>
        /// <returns>The object retrieved from TempData.</returns>
        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            tempData.TryGetValue(key, out object o);
            var val = o == null ? default : JsonConvert.DeserializeObject<T>((string)o);
            return val;
        }
    }
}