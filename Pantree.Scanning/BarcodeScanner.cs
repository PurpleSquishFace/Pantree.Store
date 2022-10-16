using Microsoft.AspNetCore.Http;
using System.Drawing;
using ZXing;
using ZXing.Common;

namespace Pantree.Scanning
{
    /// <summary>
    /// Class for reading a barcode in an image.
    /// </summary>
    public class BarcodeScanner
    {
        /// <summary>
        /// The uploaded image to be scanned and decoded.
        /// </summary>
        public IFormFile BarcodeImage { get; private set; }
        /// <summary>
        /// The options for decoding the image.
        /// </summary>
        private static DecodingOptions Options => new DecodingOptions { TryHarder = true };
        /// <summary>
        /// Whether scanning the barcode image was successful.
        /// </summary>
        public bool ScanSuccessful { get; private set; }
        /// <summary>
        /// The barcode value if found.
        /// </summary>
        public string OutputCode { get; private set; }

        /// <summary>
        /// Creates a new instance of BarcodeScanner.
        /// </summary>
        /// <param name="imageFile">The image to be scanned.</param>
        public BarcodeScanner(IFormFile imageFile)
        {
            BarcodeImage = imageFile;
            OutputCode = string.Empty;
        }

        /// <summary>
        /// Uses ZXing library to read image and attempt to read a barcode from the set image.
        /// </summary>
        /// <returns>If the process of reading the barcode image was successful.</returns>
        public bool ReadBarcode()
        {
            try
            {
                using var stream = BarcodeImage.OpenReadStream();

                var reader = new BarcodeReaderGeneric()
                {
                    Options = Options
                };

                stream.Seek(0, SeekOrigin.Begin);

                // Read the image
                using var image = (Bitmap)Image.FromStream(stream);
                LuminanceSource source;
                source = new ZXing.Windows.Compatibility.BitmapLuminanceSource(image);

                // Decode the image
                Result result = reader.Decode(source);

                // Set the output
                OutputCode = result == null ? string.Empty : result.Text;
                ScanSuccessful = result != null;
            }
            catch (Exception e)
            {
                ScanSuccessful = false;
                //Console.WriteLine(e.Message);
                // Log exception
            }

            return ScanSuccessful;
        }
    }
}