using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pantree.Scanning.Tests
{
    [TestClass()]
    public class FoodFactsAPITests
    {
        [TestMethod()]
        public void CallAPITest_Success()
        {
            // Assign
            var apiObject = new FoodFactsAPI("123456");

            // Act
            var result = apiObject.CallAPI();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(apiObject.BarcodeString, "123456");
            Assert.IsTrue(apiObject.LoadSuccessful);
            Assert.IsNotNull(apiObject.Product);
        }

        [TestMethod()]
        public void CallAPITest_Fail()
        {
            // Assign
            var apiObject = new FoodFactsAPI("999999");

            // Act
            var result = apiObject.CallAPI();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(apiObject.BarcodeString, "999999");
            Assert.IsFalse(apiObject.LoadSuccessful);
            Assert.IsNull(apiObject.Product);
        }

        [TestMethod()]
        public void CallAPITest_Empty()
        {
            // Assign
            var apiObject = new FoodFactsAPI("");

            // Act
            var result = apiObject.CallAPI();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(apiObject.BarcodeString, "");
            Assert.IsFalse(apiObject.LoadSuccessful);
            Assert.IsNull(apiObject.Product);
        }
    }
}