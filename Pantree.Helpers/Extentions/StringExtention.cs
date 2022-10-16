namespace Pantree.Helpers
{
    /// <summary>
    /// Extension methods to strings.
    /// </summary>
    public static class StringExtention
    {
        /// <summary>
        /// Takes a string and breaks down into an enumeration of strings of a set size.
        /// </summary>
        /// <param name="input">The string to be broken up.</param>
        /// <param name="maxSize">The size of each sub-string.</param>
        /// <returns>An enumerable object of the generated strings.</returns>
        public static IEnumerable<string> Divide(this string input, int maxSize)
        {
            for (int i = 0; i < input.Length; i += maxSize)
                yield return input.Substring(i, Math.Min(maxSize, input.Length - i));
        }

        /// <summary>
        /// Compresses a string.
        /// </summary>
        /// <param name="input">The string to compress.</param>
        /// <returns>The compressed string.</returns>
        public static string Compress(this string input)
        {
            return Compression.Compress(input);
        }

        /// <summary>
        /// Decrompresses a string.
        /// </summary>
        /// <param name="input">The string to decompress.</param>
        /// <returns>The decompressed string.</returns>
        public static string Decompress(this string input)
        {
            return Compression.Decompress(input);
        }
    }
}
