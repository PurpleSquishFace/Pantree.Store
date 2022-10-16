using Pantree.Data.Models;

namespace Pantree.Data.Access
{
    public interface IDataAccess_Image
    {
        /// <summary>
        /// Creates a new profile image record.
        /// </summary>
        /// <param name="profileImage">The profile image record to create.</param>
        /// <returns>The unqiue ID of the newly created record.</returns>
        int CreateProfileImage(IDatabaseTable profileImage);
        
        /// <summary>
        /// Updates a profile image record.
        /// </summary>
        /// <param name="profileImageID">The unique ID of the record to update.</param>
        /// <param name="profileImage">The new profile image to save.</param>
        /// <param name="alternativeText">The new profile image alternative text to save.</param>
        void UpdateProfileImage(int profileImageID, byte[] profileImage, string alternativeText);
        
        /// <summary>
        /// Removes a profile image record.
        /// </summary>
        /// <param name="profileImage">The profile image record to delete.</param>
        /// <returns>Whether the delete operation was successful.</returns>
        bool RemoveProfileImage(IDatabaseTable profileImage);
    }
}