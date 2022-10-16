using Microsoft.AspNetCore.Http;
using Pantree.Data.Models.Database;
using Pantree.Helpers.Extentions;
using System.Text;

namespace Pantree.Services
{
    /// <summary>
    /// A service class with methods to handle image functionality - inherits from PantreeService.
    /// </summary>
    public class ImageService : PantreeService
    {
        /// <summary>
        /// Creates a new instance of the ImageService class.
        /// </summary>
        /// <param name="dbConnectionString">The connection string to generate a database connection.</param>
        public ImageService(string dbConnectionString) : base(dbConnectionString)
        {

        }

        /// <summary>
        /// Saves a new profile image to the database for a user.
        /// </summary>
        /// <param name="userID">The User ID of the user saving the profile image.</param>
        /// <param name="profileImage">The uploaded image.</param>
        /// <param name="alternativeText">The alternative text to display if the image can't be loaded.</param>
        /// <returns>The unique ID of the newly generated database record.</returns>
        public async Task<int> AddProfileImage(int userID, IFormFile profileImage, string alternativeText)
        {
            var image = new tbl_ProfileImages
            {
                UserID = userID,
                ProfileImage = await profileImage.GetBytes(),
                AlternativeText = alternativeText
            };

            return db.CreateProfileImage(image);
        }

        /// <summary>
        /// Updates a profile image record in the database for a user.
        /// </summary>
        /// <param name="profileImageID">The unique ID of the profile image database record.</param>
        /// <param name="profileImage">The newly uploaded image.</param>
        /// <param name="alternativeText">The alternative text to display if the image can't be loaded.</param>
        public async void UpdateProfileImage(int profileImageID, IFormFile profileImage, string alternativeText)
        {
            db.UpdateProfileImage(profileImageID, await profileImage.GetBytes(), alternativeText);
        }

        /// <summary>
        /// Removes a user's profile image record from the database.
        /// </summary>
        /// <param name="profileImageID">The unique ID of the profile image database record.</param>
        public void RemoveProfileImage(int profileImageID)
        {
            var image = new tbl_ProfileImages
            {
                ProfileImageID = profileImageID
            };

            db.RemoveProfileImage(image);
        }
    }
}