using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Pantree.Scanning
{
    /// <summary>
    /// Class to handle calling the Open Food Facts api.
    /// </summary>
    public class FoodFactsAPI
    {
        /// <summary>
        /// The URL of the API to be called.
        /// </summary>
        private string ApiUrl => $"https://world.openfoodfacts.org/api/v0/product/{BarcodeString}.json";
        /// <summary>
        /// The barcode value to search.
        /// </summary>
        public string BarcodeString { get; private set; }
        /// <summary>
        /// Whether calling the API and processing the result was successful.
        /// </summary>
        public bool LoadSuccessful { get; private set; }
        /// <summary>
        /// The object containing the details of the product returned by the API call.
        /// </summary>
        public FoodFactsObject Product { get; private set; }

        /// <summary>
        /// Creates an instace of the FoodFactsAPI class, providing the barcode value to be searched.
        /// </summary>
        /// <param name="barcode">The barcode value to be searched.</param>
        public FoodFactsAPI(string barcode)
        {
            BarcodeString = barcode;
        }

        /// <summary>
        /// Calls the Open Food Facts API.
        /// </summary>
        /// <returns>Whether calling the API was successful.</returns>
        public bool CallAPI()
        {
            try
            {
                // Create the API request.
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(ApiUrl)
                };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "Pantree - Windows - Version 1.0");

                // Call the API.
                HttpResponseMessage response = client.GetAsync(ApiUrl).Result;

                // Process the API response.
                LoadSuccessful = response.IsSuccessStatusCode;

                if (LoadSuccessful)
                {
                    JObject result = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                    if (int.Parse(result["status"].ToString()) != 0)
                    {
                        // Create object holding the returned product data.
                        var productCode = result["code"].ToString();

                        var productNameObj = result["product"]["product_name"];
                        var productName = productNameObj == null ? string.Empty : productNameObj.ToString();

                        var imageUrlObj = result["product"]["image_url"];
                        var imageUrl = imageUrlObj == null ? string.Empty : imageUrlObj.ToString();

                        var ingredientListObj = result["product"]["ingredients_text"];
                        var ingredientList = ingredientListObj == null ? string.Empty : ingredientListObj.ToString();

                        Product = new FoodFactsObject(productCode, productName, imageUrl, ingredientList);
                    }
                    else
                    {
                        LoadSuccessful = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // Log exception
            }

            return LoadSuccessful;
        }
    }
}