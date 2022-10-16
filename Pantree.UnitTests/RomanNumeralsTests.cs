using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pantree.Helpers.Tests
{
    [TestClass()]
    public class RomanNumeralsTests
    {
        [TestMethod()]
        public void ToRomanNumeralTest()
        {
            // Assign
            var number = 125;

            // Act
            var numeral = number.ToRomanNumeral();

            // Assert
            Assert.AreEqual(numeral, "CXXV");
        }

        [TestMethod()]
        public void ToNumeralTest_1to10()
        {
            // Assign
            var one = 1;
            var two = 2;
            var three = 3;
            var four = 4;
            var five = 5;
            var six = 6;
            var seven = 7;
            var eight = 8;
            var nine = 9;
            var ten = 10;

            // Act
            var oneResult = RomanNumerals.ToNumeral(one);
            var twoResult = RomanNumerals.ToNumeral(two);
            var threeResult = RomanNumerals.ToNumeral(three);
            var fourResult = RomanNumerals.ToNumeral(four);
            var fiveResult = RomanNumerals.ToNumeral(five);
            var sixResult = RomanNumerals.ToNumeral(six);
            var sevenResult = RomanNumerals.ToNumeral(seven);
            var eightResult = RomanNumerals.ToNumeral(eight);
            var nineResult = RomanNumerals.ToNumeral(nine);
            var tenResult = RomanNumerals.ToNumeral(ten);

            // Assert
            Assert.AreEqual(oneResult, "I");
            Assert.AreEqual(twoResult, "II");
            Assert.AreEqual(threeResult, "III");
            Assert.AreEqual(fourResult, "IV");
            Assert.AreEqual(fiveResult, "V");
            Assert.AreEqual(sixResult, "VI");
            Assert.AreEqual(sevenResult, "VII");
            Assert.AreEqual(eightResult, "VIII");
            Assert.AreEqual(nineResult, "IX");
            Assert.AreEqual(tenResult, "X");
        }

        [TestMethod()]
        public void ToNumeralTest_11to99()
        {
            // Assign
            var one = 15;
            var two = 32;
            var three = 45;
            var four = 48;
            var five = 52;
            var six = 54;
            var seven = 55;
            var eight = 80;
            var nine = 94;
            var ten = 96;

            // Act
            var oneResult = RomanNumerals.ToNumeral(one);
            var twoResult = RomanNumerals.ToNumeral(two);
            var threeResult = RomanNumerals.ToNumeral(three);
            var fourResult = RomanNumerals.ToNumeral(four);
            var fiveResult = RomanNumerals.ToNumeral(five);
            var sixResult = RomanNumerals.ToNumeral(six);
            var sevenResult = RomanNumerals.ToNumeral(seven);
            var eightResult = RomanNumerals.ToNumeral(eight);
            var nineResult = RomanNumerals.ToNumeral(nine);
            var tenResult = RomanNumerals.ToNumeral(ten);

            // Assert
            Assert.AreEqual(oneResult, "XV");
            Assert.AreEqual(twoResult, "XXXII");
            Assert.AreEqual(threeResult, "XLV");
            Assert.AreEqual(fourResult, "XLVIII");
            Assert.AreEqual(fiveResult, "LII");
            Assert.AreEqual(sixResult, "LIV");
            Assert.AreEqual(sevenResult, "LV");
            Assert.AreEqual(eightResult, "LXXX");
            Assert.AreEqual(nineResult, "XCIV");
            Assert.AreEqual(tenResult, "XCVI");
        }

        [TestMethod()]
        public void ToNumeralTest_100to999()
        {
            // Assign
            var one = 135;
            var two = 374;
            var three = 404;
            var four = 705;
            var five = 720;
            var six = 755;
            var seven = 829;
            var eight = 860;
            var nine = 914;
            var ten = 918;

            // Act
            var oneResult = RomanNumerals.ToNumeral(one);
            var twoResult = RomanNumerals.ToNumeral(two);
            var threeResult = RomanNumerals.ToNumeral(three);
            var fourResult = RomanNumerals.ToNumeral(four);
            var fiveResult = RomanNumerals.ToNumeral(five);
            var sixResult = RomanNumerals.ToNumeral(six);
            var sevenResult = RomanNumerals.ToNumeral(seven);
            var eightResult = RomanNumerals.ToNumeral(eight);
            var nineResult = RomanNumerals.ToNumeral(nine);
            var tenResult = RomanNumerals.ToNumeral(ten);

            // Assert
            Assert.AreEqual(oneResult, "CXXXV");
            Assert.AreEqual(twoResult, "CCCLXXIV");
            Assert.AreEqual(threeResult, "CDIV");
            Assert.AreEqual(fourResult, "DCCV");
            Assert.AreEqual(fiveResult, "DCCXX");
            Assert.AreEqual(sixResult, "DCCLV");
            Assert.AreEqual(sevenResult, "DCCCXXIX");
            Assert.AreEqual(eightResult, "DCCCLX");
            Assert.AreEqual(nineResult, "CMXIV");
            Assert.AreEqual(tenResult, "CMXVIII");
        }

        [TestMethod()]
        public void ToNumeralTest_1000to9999()
        {
            // Assign
            var one = 1006;
            var two = 1523;
            var three = 2923;
            var four = 3118;
            var five = 4547;
            var six = 5268;
            var seven = 6434;
            var eight = 7158;
            var nine = 8042;
            var ten = 8346;

            // Act
            var oneResult = RomanNumerals.ToNumeral(one);
            var twoResult = RomanNumerals.ToNumeral(two);
            var threeResult = RomanNumerals.ToNumeral(three);
            var fourResult = RomanNumerals.ToNumeral(four);
            var fiveResult = RomanNumerals.ToNumeral(five);
            var sixResult = RomanNumerals.ToNumeral(six);
            var sevenResult = RomanNumerals.ToNumeral(seven);
            var eightResult = RomanNumerals.ToNumeral(eight);
            var nineResult = RomanNumerals.ToNumeral(nine);
            var tenResult = RomanNumerals.ToNumeral(ten);

            // Assert
            Assert.AreEqual(oneResult, "MVI");
            Assert.AreEqual(twoResult, "MDXXIII");
            Assert.AreEqual(threeResult, "MMCMXXIII");
            Assert.AreEqual(fourResult, "MMMCXVIII");
            Assert.AreEqual(fiveResult, "MMMMDXLVII");
            Assert.AreEqual(sixResult, "MMMMMCCLXVIII");
            Assert.AreEqual(sevenResult, "MMMMMMCDXXXIV");
            Assert.AreEqual(eightResult, "MMMMMMMCLVIII");
            Assert.AreEqual(nineResult, "MMMMMMMMXLII");
            Assert.AreEqual(tenResult, "MMMMMMMMCCCXLVI");
        }


        [TestMethod()]
        public void FromRomanNumeralTest()
        {
            // Assign
            var numeral = "CXXV";

            // Act
            var number = numeral.FromRomanNumeral();

            // Assert
            Assert.AreEqual(number, 125);
        }

        [TestMethod()]
        public void FromNumeralTest_1to10()
        {
            // Assign
            var one = "I";
            var two = "II";
            var three = "III";
            var four = "IV";
            var five = "V";
            var six = "VI";
            var seven = "VII";
            var eight = "VIII";
            var nine = "IX";
            var ten = "X";

            // Act
            var oneResult = RomanNumerals.FromNumeral(one);
            var twoResult = RomanNumerals.FromNumeral(two);
            var threeResult = RomanNumerals.FromNumeral(three);
            var fourResult = RomanNumerals.FromNumeral(four);
            var fiveResult = RomanNumerals.FromNumeral(five);
            var sixResult = RomanNumerals.FromNumeral(six);
            var sevenResult = RomanNumerals.FromNumeral(seven);
            var eightResult = RomanNumerals.FromNumeral(eight);
            var nineResult = RomanNumerals.FromNumeral(nine);
            var tenResult = RomanNumerals.FromNumeral(ten);

            // Assert
            Assert.AreEqual(oneResult, 1);
            Assert.AreEqual(twoResult, 2);
            Assert.AreEqual(threeResult, 3);
            Assert.AreEqual(fourResult, 4);
            Assert.AreEqual(fiveResult, 5);
            Assert.AreEqual(sixResult, 6);
            Assert.AreEqual(sevenResult, 7);
            Assert.AreEqual(eightResult, 8);
            Assert.AreEqual(nineResult, 9);
            Assert.AreEqual(tenResult, 10);
        }

        [TestMethod()]
        public void FromNumeralTest_11to99()
        {
            // Assign
            var one = "XIV";
            var two = "XVIII";
            var three = "XXII";
            var four = "XXVI";
            var five = "XXXVI";
            var six = "XLIX";
            var seven = "LXXIV";
            var eight = "LXXVIII";
            var nine = "LXXIX";
            var ten = "LXXXVII";

            // Act
            var oneResult = RomanNumerals.FromNumeral(one);
            var twoResult = RomanNumerals.FromNumeral(two);
            var threeResult = RomanNumerals.FromNumeral(three);
            var fourResult = RomanNumerals.FromNumeral(four);
            var fiveResult = RomanNumerals.FromNumeral(five);
            var sixResult = RomanNumerals.FromNumeral(six);
            var sevenResult = RomanNumerals.FromNumeral(seven);
            var eightResult = RomanNumerals.FromNumeral(eight);
            var nineResult = RomanNumerals.FromNumeral(nine);
            var tenResult = RomanNumerals.FromNumeral(ten);

            // Assert
            Assert.AreEqual(oneResult, 14);
            Assert.AreEqual(twoResult, 18);
            Assert.AreEqual(threeResult, 22);
            Assert.AreEqual(fourResult, 26);
            Assert.AreEqual(fiveResult, 36);
            Assert.AreEqual(sixResult, 49);
            Assert.AreEqual(sevenResult, 74);
            Assert.AreEqual(eightResult, 78);
            Assert.AreEqual(nineResult, 79);
            Assert.AreEqual(tenResult, 87);
        }

        [TestMethod()]
        public void FromNumeralTest_100to999()
        {
            // Assign
            var one = "CXVIII";
            var two = "CLXII";
            var three = "CCLI";
            var four = "CCLXXXV";
            var five = "CCCXCV";
            var six = "DLXXVII";
            var seven = "DCCXLVIII";
            var eight = "DCCCXC";
            var nine = "CMLIII";
            var ten = "CMLVI";

            // Act
            var oneResult = RomanNumerals.FromNumeral(one);
            var twoResult = RomanNumerals.FromNumeral(two);
            var threeResult = RomanNumerals.FromNumeral(three);
            var fourResult = RomanNumerals.FromNumeral(four);
            var fiveResult = RomanNumerals.FromNumeral(five);
            var sixResult = RomanNumerals.FromNumeral(six);
            var sevenResult = RomanNumerals.FromNumeral(seven);
            var eightResult = RomanNumerals.FromNumeral(eight);
            var nineResult = RomanNumerals.FromNumeral(nine);
            var tenResult = RomanNumerals.FromNumeral(ten);

            // Assert
            Assert.AreEqual(oneResult, 118);
            Assert.AreEqual(twoResult, 162);
            Assert.AreEqual(threeResult, 251);
            Assert.AreEqual(fourResult, 285);
            Assert.AreEqual(fiveResult, 395);
            Assert.AreEqual(sixResult, 577);
            Assert.AreEqual(sevenResult, 748);
            Assert.AreEqual(eightResult, 890);
            Assert.AreEqual(nineResult, 953);
            Assert.AreEqual(tenResult, 956);
        }

        [TestMethod()]
        public void FromNumeralTest_1000to9999()
        {
            // Assign
            var one = "MDLXXIV";
            var two = "MDCCCXLI";
            var three = "MMXII";
            var four = "MMCCCLXX";
            var five = "MMMMCCXXXIV";
            var six = "MMMMDCXXXIV";
            var seven = "MMMMMCCXCVI";
            var eight = "MMMMMMCDXVII";
            var nine = "MMMMMMDCCLXXV";
            var ten = "MMMMMMMDCCXL";

            // Act
            var oneResult = RomanNumerals.FromNumeral(one);
            var twoResult = RomanNumerals.FromNumeral(two);
            var threeResult = RomanNumerals.FromNumeral(three);
            var fourResult = RomanNumerals.FromNumeral(four);
            var fiveResult = RomanNumerals.FromNumeral(five);
            var sixResult = RomanNumerals.FromNumeral(six);
            var sevenResult = RomanNumerals.FromNumeral(seven);
            var eightResult = RomanNumerals.FromNumeral(eight);
            var nineResult = RomanNumerals.FromNumeral(nine);
            var tenResult = RomanNumerals.FromNumeral(ten);

            // Assert
            Assert.AreEqual(oneResult, 1574);
            Assert.AreEqual(twoResult, 1841);
            Assert.AreEqual(threeResult, 2012);
            Assert.AreEqual(fourResult, 2370);
            Assert.AreEqual(fiveResult, 4234);
            Assert.AreEqual(sixResult, 4634);
            Assert.AreEqual(sevenResult, 5296);
            Assert.AreEqual(eightResult, 6417);
            Assert.AreEqual(nineResult, 6775);
            Assert.AreEqual(tenResult, 7740);
        }
    }
}