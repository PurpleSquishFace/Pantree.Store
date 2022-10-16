using Pantree.Data.Models;
using Pantree.Data.Models.Contracts;

namespace Pantree.Data.Access
{
    internal interface IDataAccess_Friend
    {
        /// <summary>
        /// Get a list of a user's friends
        /// </summary>
        /// <param name="userID">The User ID of the user whose friends are to be returned.</param>
        /// <returns>The list of friends found.</returns>
        public List<FriendView> GetFriends(int userID);

        /// <summary>
        /// Searches for users whose username contains the provided search query.
        /// </summary>
        /// <param name="searchQuery">The search term to look up.</param>
        /// <param name="userID">The User ID of the current user conducting the search.</param>
        /// <returns>The list of users found.</returns>
        public List<UserSearchResult> SearchUsers(string searchQuery, int userID);

        /// <summary>
        /// Adds a new connection between two users.
        /// </summary>
        /// <param name="friendUser">Object containing the details of the new connection.</param>
        /// <returns>The unique ID of the newly generated record.</returns>
        public int AddUserFriend(IDatabaseTable friendUser);

        /// <summary>
        /// Gets the details of a users' friend.
        /// </summary>
        /// <typeparam name="T">The type of which the details are to be returned.</typeparam>
        /// <param name="userFriendID">The unique ID of the connection between the current user and the friend in question.</param>
        /// <returns>The details of the friend.</returns>
        public T GetUserFriend<T>(int userFriendID);

        /// <summary>
        /// Updates the status of a connection between two users.
        /// </summary>
        /// <param name="friendUser">Object containing the details with which to update the connection.</param>
        /// <returns>Whether the update operation was successful.</returns>
        public bool UpdateFriendStatus(IDatabaseTable friendUser);

        /// <summary>
        /// Removes the connection between two users.
        /// </summary>
        /// <param name="friendUser">Object containing the details of the connection to be removed.</param>
        /// <returns>Whether the delete operation was successful.</returns>
        public bool RemoveUserFriend(IDatabaseTable friendUser);
    }
}
