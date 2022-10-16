using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Web;

namespace Pantree.UnitTests
{
    public static class TestHelpers
    {
        public static IFormFile CreateMockImage(string filepath)
        {
            var stream = new MemoryStream(File.ReadAllBytes(filepath).ToArray());
            return new FormFile(stream, 0, stream.Length, "Image", filepath.Split(@"\").Last());
        }

        public static TestCookie RetrieveCookie<T>(HttpResponse response)
        {
            var setCookie = response.Headers.FirstOrDefault(i => i.Key == "Set-Cookie").Value.ToString();
            var cookieTokens = setCookie.Split(';');    
            var keyValueTokens = cookieTokens[0].Split('=');

            DateTime.TryParse(cookieTokens[1].Trim().Replace("expires=", ""), out var date);

            var value = JsonConvert.DeserializeObject<T>(HttpUtility.UrlDecode(keyValueTokens[1]));
            return new TestCookie(keyValueTokens[0], value)
            {
                Secure = cookieTokens[cookieTokens.Length - 3].Trim() == "secure",
                HttpOnly = cookieTokens[cookieTokens.Length - 1].Trim() == "httponly",
                SameSite = cookieTokens[cookieTokens.Length - 2].Trim().Replace("samesite=", ""),
                Expires = date
            };
        }

        public static void SetCookie(HttpRequest request, string key, string value, bool persistent)
        {
            var cookies = new TestCookieCollection();
            cookies.AddValue(new Cookie(key, value));

            request.Cookies = cookies;
        }
    }

    public class TestCookie
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public string SameSite { get; set; }
        public DateTime Expires { get; set; }

        public TestCookie(string key, object value)
        {
            Key = key;
            Value = value;
            SameSite = string.Empty;
        }
    }

    public class TestCookieValue
    {
        public string Foo { get; set; }
        public string Bar { get; set; }

        public TestCookieValue(string foo, string bar)
        {
            Foo = foo;
            Bar = bar;
        }
    }

    public class TestCookieCollection : IRequestCookieCollection
    {
        public string? this[string key] => GetValue(key);

        public int Count => Keys.Count;

        public ICollection<string> Keys { get; set; }

        public TestCookieCollection()
        {
            Keys = new List<string>();
        }

        public bool ContainsKey(string key)
        {
            foreach (var loopKey in Keys)
            {
                if (loopKey.Contains(key))
                    return true;
            }
            return false;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return new List<KeyValuePair<string,string>>().GetEnumerator();
        }

        public string GetValue(string key)
        {
            foreach (var loopKey in Keys)
            {
                if (loopKey.Contains(key))
                {
                    var value = loopKey.Split(';').First();
                    return value.Replace($"{key}=", "");
                }
            }
            return String.Empty;
        }

        public void AddValue(Cookie cookie)
        {
            Keys.Add($"{cookie.ToString()};path=/;secure;samesite=strict;httponly");
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out string? value)
        {
            value = null;
            if (ContainsKey(key))
            {
                foreach (var loopKey in Keys)
                {
                    if (loopKey.Contains(key))
                        value = loopKey;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var vals = (List<string>)Keys;
            return vals.Select(i => i.Substring(0, i.IndexOf("="))).ToList().GetEnumerator();
        }   
    }
}