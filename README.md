<p align="center">
  <img src="https://user-images.githubusercontent.com/48553563/196780133-a5324ee1-e1f5-4c8b-a645-569c1704c5ad.jpg" />
</p>

# Pantree

Pantree is an app designed to help keep a kitchen or food store organised, tracking the quantity of different products and where they're kept. Utilising barcode scanning to quickly add items to the app, it aims to help meal planning, reduce waste and make life easier!

The app is developed using C#, with an ASP.NET MVC project providing the user interface. The rest of the solution is broken down into components as class libraries. 

*Note: This app has been created in my personal time to demonstrate some of my technical abilities. It hasn't been robustly tested or deployed to a production environment, and as such there may be unidentified bugs or flaws.*

# Barcode Scanning

To scan barcodes in real time, the app uses two implementations of the same base library: [ZXing](https://github.com/zxing/zxing).

To allow users to scan a barcode natively within the browser, the app uses [ZXing.js](https://github.com/zxing-js/library) - a JavaScript port of the ZXing library. This will detect the barcode from the image feed directly from the device's camera, extract the code, and submits the string to the server for processing.

If scanning in the browser doesn't work - either because the user hasn't allowed the app access to the camera or the image could be processed - the user has the option to upload an image for the app to process server-side. This is implemented using [ZXing.Net](https://github.com/micjahn/ZXing.Net) - a C# port of the ZXing library.

In the case that both previous attempts to automatically read a barcode fails, there is the option for the user to manually type the code to submit. This string is then processed in the same way the code would be had it been extracted from an image.

# Barcode API

[Open Food Facts](https://world.openfoodfacts.org/) is a public, open-source database of food products. This is used to source information about products when they're scanned in. Open Food Facts provide an [API](https://wiki.openfoodfacts.org/API) to perform lookups against their database and return product information.

Pantree takes the code scanned in from a barcode, it first completes a database check to see if the current user has the product in their Pantree, if not it then completes a database lookup to check if the product has been added to any other user's Pantree, and if that doesn't return a result it then performs a lookup against the API. The API lookup is also performed intermittently based on a counter, to ensure the information is accurate and the latest data is pulled down. These steps aim to reduce the load on the API, and to minimise the amount of external lookups completed.

# Libraries

As with any application, it is build on the shoulders of giants, utilising a few different libraries:

- [Cropper.js](https://github.com/fengyuanchen/cropperjs) - This is a javascript library used to allow the user to crop images for their profile image, and ensure they are circular.
- [Newtonsoft.json](https://github.com/JamesNK/Newtonsoft.Json) - A staple of C# applications, this C# library is used to map data from a string to an object, and vice versa.
-  [Dapper](https://github.com/DapperLib/Dapper) - Created by the folks at Stack Overflow, this C# library is a powerful ORM used to map between datbase and C# objects. The backbone of the data flow between the app and the database goes through Dapper, utilising its efficient handling of paramaterised SQL queries.
-  [Mapster](https://github.com/MapsterMapper/Mapster) - Often used in conjunction with Dapper, Mapster is a nifty library that maps data between different C# objects.

# Unit Tests

Included in the solution is a Unit Test project, which tests a range of functionalities across the solution. 

# Future Developments

As this is a personal project, with no external stakeholders, development progresses at its own pace. However, there is a list of potential features that could be added to the app in the future:

- **Shopping List** - Implementing shopping list functionality, where products that run low can be added to a list, exported, and potentially updated upon purchase.
- **Use By Dates** - Adding the ability to save the Use By Dates on products, alerting the user to which products need using.
- **Merged Products** - A way of combining the same products, but from different brands - to make it easier to look up how much of an item there is regardless of the producer.

