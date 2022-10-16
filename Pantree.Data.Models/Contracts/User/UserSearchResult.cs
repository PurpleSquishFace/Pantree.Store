using Pantree.Helpers.Enums;

namespace Pantree.Data.Models.Contracts
{
    public class UserSearchResult
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public FriendStatus Status { get; set; }
        public ProfileImageView User_ProfileImage { get; set; }
    }
}