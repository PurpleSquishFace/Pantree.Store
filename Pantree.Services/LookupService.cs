using Microsoft.AspNetCore.Mvc.Rendering;
using Pantree.Data.Models.Contracts;
using Pantree.Helpers.Enums;

namespace Pantree.Services
{
    /// <summary>
    /// A service class providing values for dropdowns.
    /// </summary>
    public static class LookupService
    {
        /// <summary>
        /// Creates a dropdown list of locations, only where they have stores in them.
        /// </summary>
        /// <param name="locations">The list of locations to generate the dropdown values from.</param>
        /// <param name="locationID">The Location ID of the first location, if there is one.</param>
        /// <returns>The generated dropdown list.</returns>
        public static SelectList Locations(List<LocationView> locations, out int? locationID)
        {
            locations = locations.Where(i => i.Stores.Any()).ToList();
            locationID = locations.First()?.LocationID;

            return new SelectList(locations, "LocationID", "LocationName");
        }

        /// <summary>
        /// Creates a dropdown list of stores.
        /// </summary>
        /// <param name="stores">The list of stores to generate the dropdown values from.</param>
        /// <returns>The generated dropdown list.</returns>
        public static SelectList Stores(List<StoreView> stores)
        {
            return new SelectList(stores, "StoreID", "StoreName");
        }

        /// <summary>
        /// Creates a dropdown list of friends.
        /// </summary>
        /// <param name="friends">The list of friends to generate the dropdown values from.</param>
        /// <returns>The generated dropdown list.</returns>
        public static SelectList Friends(List<FriendView> friends)
        {
            return new SelectList(friends, "Friend_UserID", "Friend_DisplayName");
        }

        /// <summary>
        /// Creates a dropdown list of accepted friends.
        /// </summary>
        /// <param name="friends">The list of accepted friends to generate the dropdown values from.</param>
        /// <returns>The generated dropdown list.</returns>
        public static SelectList AcceptedFriends(List<FriendView> friends)
        {
            return new SelectList(friends.Where(i => i.FriendStatus == FriendStatus.Accepted), "Friend_UserID", "Friend_DisplayName");
        }
    }
}