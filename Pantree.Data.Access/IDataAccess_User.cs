using Pantree.Data.Models;
using Pantree.Data.Models.Contracts;

namespace Pantree.Data.Access
{
    internal interface IDataAccess_User
    {
        /// <summary>
        /// Looks up and returns the user based on the provided username.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="username">The username used to lookup.</param>
        /// <returns>The user object if found.</returns>
        T GetUser<T>(string username) where T : class;

        /// <summary>
        /// Creates a new user record.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        /// <returns>The ID of the newly created user.</returns>
        int CreateUser(IDatabaseTable user);

        /// <summary>
        /// Gets a user by using the unique username.
        /// </summary>
        /// <param name="userName">The username of the user to be returned.</param>
        /// <returns>The User if found.</returns>
        public PantreeUser GetUser(string userName);

        /// <summary>
        /// Gets a user by using the unique user ID.
        /// </summary>
        /// <param name="userID">The User ID of the user to be returned.</param>
        /// <returns>The User if found.</returns>
        public PantreeUser GetUser(int userID);
        
        /// <summary>
        /// Populates the provided user object with storage data.
        /// </summary>
        /// <param name="user">The user object to update.</param>
        /// <returns>The updated user object.</returns>
        public PantreeUser PopulateStorage(PantreeUser user);

        /// <summary>
        /// Populates the provided user object with friend data.
        /// </summary>
        /// <param name="user">The user object to update.</param>
        /// <returns>The updated user object.</returns>
        public PantreeUser PopulateFriends(PantreeUser user);

        /// <summary>
        /// Populates the provided object with the users linked to locations.
        /// </summary>
        /// <param name="user">The user object to update.</param>
        /// <returns>The updated user object.</returns>
        public PantreeUser PopulateLocationUsers(PantreeUser user);

        /// <summary>
        /// Updates users' display name.
        /// </summary>
        /// <param name="userID">The User ID of the user to update.</param>
        /// <param name="displayName">The display name to update the user with.</param>
        public void UpdateUserDetails(int userID, string displayName);

    }
}