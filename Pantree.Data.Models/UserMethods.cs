using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Pantree.Helpers;

namespace Pantree.Data.Models
{
    public static class UserMethods
    {
        public static User PantreeUser(User? user, HttpContext context, string encryptionKey)
        {
            User returnUser;

            if (user != null)
                returnUser = user;

            else if (context.GetCookie(out User cookieUser, encryptionKey))
                returnUser = cookieUser; 

            else
                returnUser =  new User();

            context.SetCookie(returnUser, returnUser.RememberMe, encryptionKey);

            return returnUser;
        }

        private static void SetCookie<T>(this HttpContext context, T user, bool persistCookie, string encryptionKey)
        {
            context.RemoveAllCookies();

            var sections = JsonConvert.SerializeObject(user).Divide(200).ToList();
            sections[0] = $"{sections.Count.ToRomanNumeral()}|{sections[0]}";

            for (int i = 0; i < sections.Count; i++)
                context.Response.SetEncryptedCookie($"pantree.{(i + 1).ToRomanNumeral()}", sections[i], persistCookie, encryptionKey);
        }

        private static bool GetCookie<T>(this HttpContext context, out T value, string encryptionKey)
        {
            string userString = "";

            var firstCookie = context.Request.GetEncryptedCookie<string>($"pantree.{1.ToRomanNumeral()}", encryptionKey);
            var count = firstCookie == null ? 0 : firstCookie[..firstCookie.IndexOf('|')].FromRomanNumeral();

            for (int i = 0; i < count; i++) 
                userString += context.Request.GetEncryptedCookie<string>($"pantree.{(i + 1).ToRomanNumeral()}", encryptionKey);

            try
            {
                value = JsonConvert.DeserializeObject<T>(userString[(count.ToRomanNumeral().Length + 1)..]);
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }
    }
}