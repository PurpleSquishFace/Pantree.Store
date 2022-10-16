using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pantree.UnitTests;

namespace Pantree.Scanning.Tests
{
    [TestClass()]
    public class BarcodeScannerTests
    {
        [TestMethod()]
        public void ReadBarcodeTest()
        {
            // Assign
            var file = TestHelpers.CreateMockImage("TestResources/barcode.jpg");
            var barcodeScanner = new BarcodeScanner(file);

            // Act
            var result = barcodeScanner.ReadBarcode();

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(barcodeScanner.ScanSuccessful);
            Assert.AreEqual(barcodeScanner.OutputCode, "123456");
        }

        [TestMethod()]
        public void ReadBarcodeTest_Null()
        {
            // Assign
            var barcodeScanner = new BarcodeScanner(null);

            // Act
            var result = barcodeScanner.ReadBarcode();

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(barcodeScanner.ScanSuccessful);
            Assert.AreEqual(barcodeScanner.OutputCode, string.Empty);
        }
    }
}