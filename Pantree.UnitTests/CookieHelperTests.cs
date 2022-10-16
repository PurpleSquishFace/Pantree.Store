using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Pantree.UnitTests;
using System.Net;
using System.Web;

namespace Pantree.Helpers.Tests
{
    [TestClass()]
    public class CookieHelperTests
    {
        [TestMethod()]
        public void SetCookieTest_Persistant()
        {
            // Assign
            var context = new DefaultHttpContext();
            var value = new TestCookieValue("foo", "bar");
            var key = "CookieKey";
            var persistCookie = true;

            // Act
            CookieHelper.SetCookie(context.Response, key, value, persistCookie);

            var cookie = TestHelpers.RetrieveCookie<TestCookieValue>(context.Response);
            var retrievedValue = (TestCookieValue)cookie.Value;

            // Assert
            Assert.AreEqual(key, cookie.Key);
            Assert.IsInstanceOfType(cookie.Value, typeof(TestCookieValue));
            Assert.AreEqual(retrievedValue.Foo, "foo");
            Assert.AreEqual(retrievedValue.Bar, "bar");
            Assert.IsTrue(cookie.Secure);
            Assert.IsTrue(cookie.HttpOnly);
            Assert.AreEqual(cookie.SameSite, "strict");
            Assert.AreEqual(cookie.Expires.Date, DateTime.Now.AddMonths(6).Date);
        }

        [TestMethod()]
        public void SetCookieTest_NonPersistant()
        {
            // Assign
            var context = new DefaultHttpContext();
            var value = new TestCookieValue("foo", "bar");
            var key = "CookieKey";
            var persistCookie = false;

            // Act
            CookieHelper.SetCookie(context.Response, key, value, persistCookie);

            var cookie = TestHelpers.RetrieveCookie<TestCookieValue>(context.Response);
            var retrievedValue = (TestCookieValue)cookie.Value;

            // Assert
            Assert.AreEqual(key, cookie.Key);
            Assert.IsInstanceOfType(cookie.Value, typeof(TestCookieValue));
            Assert.AreEqual(retrievedValue.Foo, "foo");
            Assert.AreEqual(retrievedValue.Bar, "bar");
            Assert.IsTrue(cookie.Secure);
            Assert.IsTrue(cookie.HttpOnly);
            Assert.AreEqual(cookie.SameSite, "strict");
            Assert.AreEqual(cookie.Expires, DateTime.MinValue);
        }

        [TestMethod()]
        public void GetCookieTest()
        {
            // Assign
            var context = new DefaultHttpContext();
            var value = new TestCookieValue("foo", "bar");
            var key = "CookieKey";
            var persistCookie = false;

            TestHelpers.SetCookie(context.Request, key, JsonConvert.SerializeObject(value), persistCookie);

            // Act
            var output = CookieHelper.GetCookie<TestCookieValue>(context.Request, key);

            // Assert
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(TestCookieValue));
            Assert.AreEqual(output.Foo, "foo");
            Assert.AreEqual(output.Bar, "bar");
        }

        [TestMethod()]
        public void SetEncryptedCookieTest_Persistent()
        {
            // Assign
            var context = new DefaultHttpContext();
            var value = new TestCookieValue("foo", "bar");
            var key = "CookieKey";
            var persistCookie = true;
            var encryptionKey = "abc123";

            // Act
            CookieHelper.SetEncryptedCookie(context.Response, key, value, persistCookie, encryptionKey);

            var cookie = TestHelpers.RetrieveCookie<string>(context.Response);
            var retrievedValue = (string)cookie.Value;

            // Assert
            Assert.AreEqual(key, cookie.Key);
            Assert.IsInstanceOfType(cookie.Value, typeof(string));
            Assert.AreEqual(retrievedValue, "gxWksDB9wucQFPpLJcjA8lDe1mM9mIw+QER44V2ThJQYgUfTmiUKA+9Z8ANFcqMxwk9EPNm0hthS0Ugi5fzbSmvwLVRky9LoGsX1BVGuyXI=");
            Assert.IsTrue(cookie.Secure);
            Assert.IsTrue(cookie.HttpOnly);
            Assert.AreEqual(cookie.SameSite, "strict");
            Assert.AreEqual(cookie.Expires.Date, DateTime.Now.AddMonths(6).Date);
        }

        [TestMethod()]
        public void SetEncryptedCookieTest_NonPersistent()
        {
            // Assign
            var context = new DefaultHttpContext();
            var value = new TestCookieValue("foo", "bar");
            var key = "CookieKey";
            var persistCookie = false;
            var encryptionKey = "abc123";

            // Act
            CookieHelper.SetEncryptedCookie(context.Response, key, value, persistCookie, encryptionKey);

            var cookie = TestHelpers.RetrieveCookie<string>(context.Response);
            var retrievedValue = (string)cookie.Value;

            // Assert
            Assert.AreEqual(key, cookie.Key);
            Assert.IsInstanceOfType(cookie.Value, typeof(string));
            Assert.AreEqual(retrievedValue, "gxWksDB9wucQFPpLJcjA8lDe1mM9mIw+QER44V2ThJQYgUfTmiUKA+9Z8ANFcqMxwk9EPNm0hthS0Ugi5fzbSmvwLVRky9LoGsX1BVGuyXI=");
            Assert.IsTrue(cookie.Secure);
            Assert.IsTrue(cookie.HttpOnly);
            Assert.AreEqual(cookie.SameSite, "strict");
            Assert.AreEqual(cookie.Expires, DateTime.MinValue);
        }

        [TestMethod()]
        public void GetEncryptedCookieTest()
        {
            // Assign
            var context = new DefaultHttpContext();
            var key = "CookieKey";
            var value = new TestCookieValue("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "Etiam efficitur aliquam odio, non efficitur ex semper at.");
            var encrypted = Encryption.Encrypt(JsonConvert.SerializeObject(value).Compress(), "abc123");

            TestHelpers.SetCookie(context.Request, key, encrypted, false);

            // Act
            var output = CookieHelper.GetEncryptedCookie<TestCookieValue>(context.Request, key, "abc123");

            // Assert
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(TestCookieValue));
            Assert.AreEqual(output.Foo, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
            Assert.AreEqual(output.Bar, "Etiam efficitur aliquam odio, non efficitur ex semper at.");
        }
    }
}