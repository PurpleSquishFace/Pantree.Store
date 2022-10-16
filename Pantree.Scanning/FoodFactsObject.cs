namespace Pantree.Scanning
{
    /// <summary>
    /// An object to hold the values returned from calling the Open Food Facts API.
    /// </summary>
    public class FoodFactsObject
    {
        /// <summary>
        /// The code of the product, commonly the barcode value.
        /// </summary>
        public string ProductCode { get; private set; }
        /// <summary>
        /// The name of the product - defaults to "No product name found";
        /// </summary>
        public string ProductName { get; private set; }
        /// <summary>
        /// The URL of an image of the product - defaults to a URL of a placeholder image.
        /// </summary>
        public string ImageURL { get; private set; }
        /// <summary>
        /// The list of ingredients in the product - defaults to "No ingredient list found".
        /// </summary>
        public string IngredientList { get; private set; }

        /// <summary>
        /// Creates an instance of FoodFactsObject, populating the associated fields.
        /// </summary>
        /// <param name="productCode">The code of the product, commonly the barcode value.</param>
        /// <param name="productName">The name of the product.</param>
        /// <param name="imageUrl">The url of an image of the product.</param>
        /// <param name="ingredientList">The list of ingredients in the product.</param>
        public FoodFactsObject(string productCode, string? productName, string? imageUrl, string? ingredientList)
        {
            ProductCode = productCode;
            ProductName = string.IsNullOrWhiteSpace(productName) ? "No product name found" : productName;
            ImageURL = string.IsNullOrWhiteSpace(imageUrl) ? "/Images/Placeholder.jpg" : imageUrl;
            IngredientList = string.IsNullOrWhiteSpace(ingredientList) ? "No ingredient list found" : ingredientList;
        }
    }
}