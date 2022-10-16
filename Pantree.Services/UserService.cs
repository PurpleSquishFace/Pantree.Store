using Microsoft.AspNetCore.Http;
using Pantree.Data.Models;
using Pantree.Data.Models.Contracts;
using Pantree.Data.Models.Database;
using Pantree.Helpers;
using Pantree.Helpers.Enums;

namespace Pantree.Services
{
    /// <summary>
    /// A service class with methods to handle user functionality - inherits from PantreeService.
    /// </summary>
    public class UserService : PantreeService
    {
        /// <summary>
        /// The loaded user data.
        /// </summary>
        public User UserModel { get; set; }

        /// <summary>
        /// Creates a new instance of the UserService class.
        /// </summary>
        /// <param name="dbConnectionString">The connection string to generate a database connection.</param>
        public UserService(string dbConnectionString) : base(dbConnectionString)
        {
            UserModel = new User();
        }

        /// <summary>
        /// Creates a new instance of the UserService class, and loads a user into the UserModel property.
        /// </summary>
        /// <param name="username">The username of the user to load.</param>
        /// <param name="password">The password of the user to load.</param>
        /// <param name="dbConnectionString">The connection string to generate a database connection.</param>
        public UserService(string username, string password, string dbConnectionString) : base(dbConnectionString)
        {
            UserModel = new User();
            LoadUser(username, password);
        }

        /// <summary>
        /// Load a user from the database, if the provided credentials are correct and can be authenticated.
        /// </summary>
        /// <param name="username">The username of the user to load.</param>
        /// <param name="password">The password of the user to load.</param>
        /// <returns>If the user was loaded and authenticated.</returns>
        public bool LoadUser(string username, string password)
        {
            var user = db.GetUser<tbl_Users>(username);
            var IsAuthenticated = user != null && SecurePassword.Verify(password, user.PasswordHash);

            if (IsAuthenticated)
            {
                UserModel = new User()
                {
                    IsAuthenticated = IsAuthenticated,
                    UserID = user.UserID,
                    Username = user.Username,
                    Name = user.DisplayName,
                    EmailAddress = user.EmailAddress                    
                };
            }

            return IsAuthenticated;
        }

        /// <summary>
        /// Creates a new user record in the database, with the values provided and creating a password hash.
        /// </summary>
        /// <param name="username">The username of the new user.</param>
        /// <param name="password">The password of the new user.</param>
        /// <param name="name">The display name of the new user.</param>
        /// <param name="emailAddress">The email address of the new user.</param>
        /// <returns>If the user was successful created and loaded.</returns>
        public bool CreateUser(string username, string password, string name, string emailAddress)
        {
            var user = new tbl_Users()
            {
                Username = username,
                PasswordHash = SecurePassword.Hash(password),
                DisplayName = name,
                EmailAddress = emailAddress
            };

            db.CreateUser(user);
            return LoadUser(username, password);
        }

        /// <summary>
        /// Updates the cookie storing the current user data, with the latest values from the database.
        /// </summary>
        /// <param name="context">The current HttpConetext.</param>
        /// <param name="user">The user to be updated.</param>
        /// <param name="encryptionKey">The key to encrypt the cookie data with.</param>
        /// <returns>The loaded user details.</returns>
        public PantreeUser UpdateUserCookies(HttpContext context, User user, string encryptionKey)
        {
            var loadUser = db.GetUser<tbl_Users>(user.Username);

            var userModel = new User()
            {
                IsAuthenticated = user.IsAuthenticated,
                UserID = loadUser.UserID,
                Username = loadUser.Username,
                Name = loadUser.DisplayName,
                EmailAddress = loadUser.EmailAddress
            };

            UserMethods.PantreeUser(userModel, context, encryptionKey);

            return db.GetUser(user.UserID);
        }

        /// <summary>
        /// Get the full user details based on a username.
        /// </summary>
        /// <param name="userName">The username of the user to load.</param>
        /// <returns>The loaded user details.</returns>
        public PantreeUser GetFullUserData(string userName)
        {
            return db.GetUser(userName);
        }

        /// <summary>
        /// Update the display name of a user, and updates the user cookie.
        /// </summary>
        /// <param name="context">The current HttpContext.</param>
        /// <param name="details">The updated values to update the details with.</param>
        /// <param name="user">The current user details.</param>
        /// <param name="encryptionKey">The key to encrypt the cookie data with.</param>
        /// <returns>The loaded updated user details.</returns>
        public PantreeUser UpdateUserDetails(HttpContext context, UserEdit details, User user, string encryptionKey)
        {
            db.UpdateUserDetails(details.UserID, details.DisplayName);
            return UpdateUserCookies(context, user, encryptionKey);
        }

        /// <summary>
        /// Get the friends linked to a user, including their details such as name, status, and profile image.
        /// </summary>
        /// <param name="userID">The User ID of the user to search friends against.</param>
        /// <returns>A list of the friends that were found, if any.</returns>
        public List<FriendView> GetFriends(int userID)
        {
            return db.GetFriends(userID);
        }

        /// <summary>
        /// Searches for users bases on the provided search query, excluding any users that have been blocked or blocked the current user.
        /// </summary>
        /// <param name="searchQuery">The search term to look up users against.</param>
        /// <param name="userID">The User ID of the current user.</param>
        /// <returns>A list of users that were found.</returns>
        public List<UserSearchResult> SearchUsers(string searchQuery, int userID)
        {
            return db.SearchUsers(searchQuery, userID).Where(i => i.Status != FriendStatus.Blocked).ToList();
        }

        /// <summary>
        /// Sends a friend request to a user, adding the record in the database.
        /// </summary>
        /// <param name="userID">The User ID of the friend to send the request to.</param>
        /// <param name="currentUserID">The User ID of the current user sending the request.</param>
        /// <returns>The unique ID of the newly created database record.</returns>
        public int AddFriendRequest(int userID, int currentUserID)
        {
            var userFriend = new tbl_UserFriends
            {
                UserID_Requester = currentUserID,
                UserID_Addressee = userID,
                FriendStatusID = (int)FriendStatus.Requested,
                DateRequested = DateTime.Now
            };

            return db.AddUserFriend(userFriend);
        }

        /// <summary>
        /// Updates the status of a friend record in the database.
        /// </summary>
        /// <param name="userFriendID">The unqiue ID of the record storing the friend data.</param>
        /// <param name="userID">The User ID of the current user.</param>
        /// <param name="status">The new friend status.</param>
        /// <returns>Whether updating the friend status was successful.</returns>
        public bool UpdateUserFriendStatus(int userFriendID, int userID, FriendStatus status)
        {
            var record = db.GetUserFriend<tbl_UserFriends>(userFriendID);
            record.FriendStatusID = (int)status;

            if (record.UserID_Requester != userID)
            {
                record.UserID_Addressee = record.UserID_Requester;
                record.UserID_Requester = userID;
            }

            if (status == FriendStatus.Accepted)
            {
                record.DateAccepted = DateTime.Now;
            }

            return db.UpdateFriendStatus(record);
        }

        /// <summary>
        /// Removes friend details record, effectively unfriending a user.
        /// </summary>
        /// <param name="userFriendID">The unique ID of the record storing the friend data.</param>
        /// <returns>Whether removing the record was successful.</returns>
        public bool RemoveUserFriend(int userFriendID)
        {
            var userFriend = new tbl_UserFriends
            {
                UserFriendID = userFriendID
            };

            return db.RemoveUserFriend(userFriend);
        }
    }
}