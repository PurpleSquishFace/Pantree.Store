using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pantree.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pantree.Helpers.Tests
{
    [TestClass()]
    public class EncryptionTests
    {
        [TestMethod()]
        public void EncryptTest()
        {
            // Assign
            var key = "abc123";
            var input = "TestString";

            // Act
            var output = Encryption.Encrypt(input, key);

            // Assert
            Assert.AreEqual(output, "RY6khI9hIM8x3QpMwu9bFBsFVaYXSlcmnpOClkpHp84=");
        }

        [TestMethod()]
        public void EncryptTest_EmptyKey()
        {
            // Assign
            var key = "";
            var input = "TestString";

            // Act
            var output = Encryption.Encrypt(input, key);

            // Assert
            Assert.AreEqual(output, "xBeBmA7Lkjz7Nc3jXGKgtYdlMzy7EFf0aq2e3+HmoD4=");
        }

        [TestMethod()]
        public void EncryptTest_EmptyInput()
        {
            // Assign
            var key = "abc123";
            var input = "";

            // Act
            var output = Encryption.Encrypt(input, key);

            // Assert
            Assert.AreEqual(output, "NM/roEnkFlHAP3i+NEBvdg==");
        }


        [TestMethod()]
        public void DecryptTest()
        {
            // Assign
            var key = "abc123";
            var input = "RY6khI9hIM8x3QpMwu9bFBsFVaYXSlcmnpOClkpHp84=";

            // Act
            var output = Encryption.Decrypt(input, key);

            // Assert
            Assert.AreEqual(output, "TestString");
        }

        [TestMethod()]
        public void DecryptTest_EmptyKey()
        {
            // Assign
            var key = "";
            var input = "xBeBmA7Lkjz7Nc3jXGKgtYdlMzy7EFf0aq2e3+HmoD4=";

            // Act
            var output = Encryption.Decrypt(input, key);

            // Assert
            Assert.AreEqual(output, "TestString");
        }

        [TestMethod()]
        public void DecryptTest_EmptyInput()
        {
            // Assign
            var key = "abc123";
            var input = "NM/roEnkFlHAP3i+NEBvdg==";

            // Act
            var output = Encryption.Decrypt(input, key);

            // Assert
            Assert.AreEqual(output, "");
        }
    }
}