using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace Pantree.Helpers.Extentions
{
    /// <summary>
    /// Adds ability to get/set complex objects to the current session, by parsing the object to and from a Json string.
    /// </summary>
    public static class SessionExtentions
    {
        /// <summary>
        /// Sets an object to the current session.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="session">Instance of the current session.</param>
        /// <param name="key">The name with which the object is stored and be retrieved.</param>
        /// <param name="value">The object to be persisted in session.</param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Gets an object from the current session.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="session">Instance of the current session.</param>
        /// <param name="key">The name with which to retrieve the object from the session</param>
        /// <returns>The object found in the session. If nothing is found, the default value of the object is returned.</returns>
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}