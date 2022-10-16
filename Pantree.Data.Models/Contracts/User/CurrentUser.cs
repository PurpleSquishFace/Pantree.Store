using System.ComponentModel.DataAnnotations;

namespace Pantree.Data.Models.Contracts
{
    public class PantreeUser
    {
        public int UserID { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Email")]
        public string EmailAddress { get; set; }
        public ProfileImageView ProfileImage { get; set; }
        public List<LocationView> Locations { get; set; }
        public List<LocationView> SharedLocations { get; set; }
        public List<FriendView> Friends { get; set; }
    }
}