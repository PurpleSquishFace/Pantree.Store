using Pantree.Helpers.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pantree.Data.Models.Contracts
{
    public class LocationShare
    {
        public int LocationID { get; set; }

        [Display(Name = "Share with")]
        [Required]
        public int UserID { get; set; }
        public List<SharedUserView> SharedUsers { get; set; }
        public SelectList Friends { get; set; }

        public LocationShare(int userID, List<SharedUserView> sharedUsers, SelectList friends)
        {
            UserID = userID;
            this.Friends = friends;

            if(sharedUsers != null)
            {
                var remove = sharedUsers.First(i => i.UserID == UserID);
                if (remove != null) sharedUsers.Remove((SharedUserView)remove);
            }            

            this.SharedUsers = sharedUsers ?? new List<SharedUserView>();
        }
    }
}