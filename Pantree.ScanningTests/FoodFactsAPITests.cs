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
    public class FoodFactsAPITests
    {
        [TestMethod()]
        public void CallAPITest()
        {
            // Assign

            var apiObject = new FoodFactsAPI("12345");

            // Act

            var result = apiObject.CallAPI();

            // Assert

            Assert.AreEqual(result, true);
        }
    }
}