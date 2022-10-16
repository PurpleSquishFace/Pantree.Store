namespace Pantree.Helpers
{
    /// <summary>
    /// Class to generate a random string.
    /// </summary>
    public static class RandomString
    {
        private static Random random = new Random();

        /// <summary>
        /// Generates a random string, made of capital letters and numbers 0-9.
        /// </summary>
        /// <param name="length">The length of the random string being generated.</param>
        /// <returns>The generated string.</returns>
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)])
              .ToArray());
        }
    }
}