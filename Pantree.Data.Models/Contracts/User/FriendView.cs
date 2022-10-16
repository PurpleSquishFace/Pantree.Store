using Pantree.Helpers.Enums;

namespace Pantree.Data.Models.Contracts
{
    public class FriendView
    {
        public int UserFriendID { get; set; }
        public int UserID_Requester { get; set; }
        public int Friend_UserID { get; set; }
        public string Friend_UserName { get; set; }
        public string Friend_DisplayName { get; set; }
        public FriendStatus FriendStatus { get; set; }
        public ProfileImageView Friend_ProfileImage { get; set; }
    }
}