using System.IO.Compression;
using System.Text;

namespace Pantree.Helpers
{
    /// <summary>
    /// Simple class to compress and decompress strings.
    /// </summary>
    public static class Compression
    {
        /// <summary>
        /// Compresses a string.
        /// </summary>
        /// <param name="input">The uncompressed input string.</param>
        /// <returns>The compressed output string.</returns>
        public static string Compress(string input)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(input);
            byte[] compressed = Compress(encoded);
            return Convert.ToBase64String(compressed);
        }

        /// <summary>
        /// Decompresses a string.
        /// </summary>
        /// <param name="input">The compressed input string.</param>
        /// <returns>The uncompressed output string, or an empty string if decompression fails.</returns>
        public static string Decompress(string input)
        {
            try
            {
                byte[] compressed = Convert.FromBase64String(input);
                byte[] decompressed = Decompress(compressed);
                return Encoding.UTF8.GetString(decompressed);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Private compression method using GZipStream.
        /// </summary>
        /// <param name="input">The data to be compressed.</param>
        /// <returns>The compressed data.</returns>
        private static byte[] Compress(byte[] input)
        {
            using var memoryStream = new MemoryStream();
            using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
            {
                gzipStream.Write(input, 0, input.Length);
            }
            
            var output = memoryStream.ToArray();

            return output.Length > input.Length ? input : output;
        }

        /// <summary>
        /// Private decompression method using GZipStream.
        /// </summary>
        /// <param name="input">The data to be decompressed.</param>
        /// <returns>The decompressed data.</returns>
        private static byte[] Decompress(byte[] input)
        {
            using var memoryStream = new MemoryStream(input);
            using var outputStream = new MemoryStream();
            using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                decompressStream.CopyTo(outputStream);
            }
            return outputStream.ToArray();
        }
    }
}