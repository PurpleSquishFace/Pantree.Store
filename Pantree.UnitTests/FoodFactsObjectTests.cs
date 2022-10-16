using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pantree.Scanning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pantree.Scanning.Tests
{
    [TestClass()]
    public class FoodFactsObjectTests
    {
        [TestMethod()]
        public void FoodFactsObjectTest()
        {
            // Assign/Act
            var foodFactsObject = new FoodFactsObject("123456", "Test Product", "https://www.image.com", "Item 1, Item 2, Item 3");

            // Assert
            Assert.AreEqual(foodFactsObject.ProductCode, "123456");
            Assert.AreEqual(foodFactsObject.ProductName, "Test Product");
            Assert.AreEqual(foodFactsObject.ImageURL, "https://www.image.com");
            Assert.AreEqual(foodFactsObject.IngredientList, "Item 1, Item 2, Item 3");
        }

        [TestMethod()]
        public void FoodFactsObjectTest_Empty()
        {
            // Assign/Act
            var foodFactsObject = new FoodFactsObject("123456", "", "", "");

            // Assert
            Assert.AreEqual(foodFactsObject.ProductCode, "123456");
            Assert.AreEqual(foodFactsObject.ProductName, "No product name found");
            Assert.AreEqual(foodFactsObject.ImageURL, "/Images/Placeholder.jpg");
            Assert.AreEqual(foodFactsObject.IngredientList, "No ingredient list found");
        }

        [TestMethod()]
        public void FoodFactsObjectTest_Null()
        {
            // Assign/Act
            var foodFactsObject = new FoodFactsObject("123456", null, null, null);

            // Assert
            Assert.AreEqual(foodFactsObject.ProductCode, "123456");
            Assert.AreEqual(foodFactsObject.ProductName, "No product name found");
            Assert.AreEqual(foodFactsObject.ImageURL, "/Images/Placeholder.jpg");
            Assert.AreEqual(foodFactsObject.IngredientList, "No ingredient list found");
        }
    }
}