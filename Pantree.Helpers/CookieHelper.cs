using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Pantree.Helpers
{
    /// <summary>
    /// Class to handle getting, setting, and deleting browser cookies
    /// </summary>
    public static class CookieHelper
    {
        /// <summary>
        /// Sets a cookie.
        /// </summary>
        /// <typeparam name="T">The type of the object being stored in the cookie.</typeparam>
        /// <param name="response">The current HttpResponse.</param>
        /// <param name="key">The unique name of the cookie.</param>
        /// <param name="value">The data to be saved in the cookie.</param>
        /// <param name="persistCookie">Flag for whether the cookie should disappear at the end of the session, or persist in the browser.</param>
        public static void SetCookie<T>(this HttpResponse response, string key, T value, bool persistCookie)
        {
            CookieOptions options = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
                Expires = persistCookie ? DateTime.Now.AddMonths(6) : default(DateTime?)
            };

            response.Cookies.Append(key, JsonConvert.SerializeObject(value), options);
        }

        /// <summary>
        /// Gets a cookie.
        /// </summary>
        /// <typeparam name="T">The type of the object stored in the cookie.</typeparam>
        /// <param name="request">The current HttpRequest.</param>
        /// <param name="key">The unique name of the cookie.</param>
        /// <returns>The object stored in the cookie, if found.</returns>
        public static T? GetCookie<T>(this HttpRequest request, string key)
        {
            var value = request.Cookies[key];

            try
            {
                if (typeof(T) == typeof(string))
                    return (T)Convert.ChangeType(value, typeof(T));
                else
                    return JsonConvert.DeserializeObject<T>(value);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Sets a cookie, compressing and encrypting the data first.
        /// </summary>
        /// <typeparam name="T">The type of the object being stored in the cookie.</typeparam>
        /// <param name="response">The current HttpResponse.</param>
        /// <param name="key">The unqiue name of the cookie.</param>
        /// <param name="value">The data to be saved in the cookie.</param>
        /// <param name="persistCookie">Flag for whether the cookie should disappear at the end of the session, or persist in the browser.</param>
        /// <param name="encryptionKey">The key used to encrypt the cookie data.</param>
        public static void SetEncryptedCookie<T>(this HttpResponse response, string key, T value, bool persistCookie, string encryptionKey)
        {
            var content = JsonConvert.SerializeObject(value);
            var encryptedContent = Encryption.Encrypt(content.Compress(), encryptionKey);

            SetCookie(response, key, encryptedContent, persistCookie);
        }

        /// <summary>
        /// Gets a cookie, then decrypts and decompresses the data.
        /// </summary>
        /// <typeparam name="T">The type of the object stored in the cookie.</typeparam>
        /// <param name="request">The current HttpRequest.</param>
        /// <param name="key">The unique name of the cookie.</param>
        /// <param name="encryptionKey">The key used when encrypting the data, to decrypt the cookie.</param>
        /// <returns>The object stored in the cookie, if found.</returns>
        public static T GetEncryptedCookie<T>(this HttpRequest request, string key, string encryptionKey)
        {
            var data = GetCookie<string>(request, key);
            var decryptedData = data == null? null : Encryption.Decrypt(data, encryptionKey);
            return decryptedData == null ? default : JsonConvert.DeserializeObject<T>(decryptedData.Decompress());
        }
        
        /// <summary>
        /// Removes all cookies created by the Pantree app.
        /// </summary>
        /// <param name="context">The current HttpContext</param>
        public static void RemoveAllCookies(this HttpContext context)
        {
            foreach (var key in context.Request.Cookies.Keys)
                if(key.Contains("pantree"))
                    context.Response.Cookies.Delete(key);
        }
    }
}