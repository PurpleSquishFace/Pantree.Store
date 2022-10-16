using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pantree.Helpers.Tests
{
    [TestClass()]
    public class RandomStringTests
    {
        [TestMethod()]
        public void GetRandomStringTest()
        {
            // Assign
            var maxLength = 250;

            for (int i = 1; i <= maxLength; i++)
            {
                // Act
                var randomString = RandomString.GetRandomString(i);

                // Assert
                Assert.AreEqual(randomString.Length, i);
                Assert.IsFalse(randomString.Any(char.IsLower));
            }            
        }
    }
}