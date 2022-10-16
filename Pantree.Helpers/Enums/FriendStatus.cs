namespace Pantree.Helpers.Enums
{
    /// <summary>
    /// Enum for the different statuses a users' friend can be.
    /// </summary>
    public enum FriendStatus
    {
        /// <summary>
        /// A friend that has been requested, but not yet accepted.
        /// </summary>
        Requested = 1,
        /// <summary>
        /// A friend that has been accepted.
        /// </summary>
        Accepted = 2,
        /// <summary>
        /// A friend that has the friend request declined.
        /// </summary>
        Declined = 3,
        /// <summary>
        /// A friend that has been blocked by a user and cannot be viewed.
        /// </summary>
        Blocked = 4,
        /// <summary>
        /// A friend that is currently an accepted friend.
        /// </summary>
        CurrentUser = 5,
        /// <summary>
        /// Another user that is currently not in any other status.
        /// </summary>
        NotFriends = 6
    }
}