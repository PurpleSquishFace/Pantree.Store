using Microsoft.AspNetCore.Http;

namespace Pantree.Helpers.Extentions
{
    public static class FormFileExtension
    {
        /// <summary>
        /// Generates a byte array of a file uploaded from the browser.
        /// </summary>
        /// <param name="formFile">The uploaded file.</param>
        /// <returns>The byte array of the uploaded file.</returns>
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}