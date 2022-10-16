using System.Text;

namespace Pantree.Helpers
{
    /// <summary>
    /// Class with methods to convert between numbers and roman numerals.
    /// </summary>
    public static class RomanNumerals
    {
        public static readonly List<KeyValuePair<char, int>> NumeralToNumber;
        public static readonly List<KeyValuePair<int, string>> NumberToNumeral;

        static RomanNumerals()
        {
            NumeralToNumber = new List<KeyValuePair<char, int>>
            {
                new KeyValuePair<char, int>('I', 1),
                new KeyValuePair<char, int>('V', 5 ),
                new KeyValuePair<char, int>('X', 10 ),
                new KeyValuePair<char, int>('L', 50 ),
                new KeyValuePair<char, int>('C', 100 ),
                new KeyValuePair<char, int>('D', 500 ),
                new KeyValuePair<char, int>('M', 1000 )
            };

            NumberToNumeral = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(1000, "M"),
                new KeyValuePair<int, string>(900, "CM"),
                new KeyValuePair<int, string>(500, "D"),
                new KeyValuePair<int, string>(400, "CD"),
                new KeyValuePair<int, string>(100, "C"),
                new KeyValuePair<int, string>(90, "XC"),
                new KeyValuePair<int, string>(50, "L"),
                new KeyValuePair<int, string>(40, "XL"),
                new KeyValuePair<int, string>(10, "X"),
                new KeyValuePair<int, string>(9, "IX"),
                new KeyValuePair<int, string>(5, "V"),
                new KeyValuePair<int, string>(4, "IV"),
                new KeyValuePair<int, string>(1, "I")
            };
        }

        /// <summary>
        /// Extension method to convert an integer to roman numerals.
        /// </summary>
        /// <param name="number">The number to be converted.</param>
        /// <returns>The generated roman numeral.</returns>
        public static string ToRomanNumeral(this int number)
        {
            return ToNumeral(number);
        }

        /// <summary>
        /// Converts an integer to roman numerals.
        /// </summary>
        /// <param name="number">The number to be converted.</param>
        /// <returns>The generated roman numeral.</returns>
        public static string ToNumeral(int number)
        {
            var numeral = new StringBuilder();

            foreach (var val in NumberToNumeral)
            {
                while (number >= val.Key)
                {
                    numeral.Append(val.Value);
                    number -= val.Key;
                }
            }

            return numeral.ToString();
        }

        /// <summary>
        /// Extension method to convert roman numerals to an integer.
        /// </summary>
        /// <param name="numeral">The roman numerals to be converted.</param>
        /// <returns>The converted number.</returns>
        public static int FromRomanNumeral(this string numeral)
        {
            return FromNumeral(numeral);
        }

        /// <summary>
        /// Converts roman numerals to an integer.
        /// </summary>
        /// <param name="numeral">The roman numerals to be converted.</param>
        /// <returns>The converted number.</returns>
        public static int FromNumeral(string numeral)
        {
            int number = 0;

            int cur, prev = 0;
            char curNum, prevNum = '\0';

            for (int i = 0; i < numeral.Length; i++)
            {
                curNum = numeral[i];

                prev = prevNum != '\0' ? NumeralToNumber.FirstOrDefault(i => i.Key == prevNum).Value : '\0';
                cur = NumeralToNumber.FirstOrDefault(i => i.Key == curNum).Value;

                if (prev != 0 && cur > prev)
                    number = number - (2 * prev) + cur;
                else
                    number += cur;

                prevNum = curNum;
            }

            return number;
        }
    }
}