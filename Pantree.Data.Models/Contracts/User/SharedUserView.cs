namespace Pantree.Data.Models.Contracts
{
    public class SharedUserView
    {
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public ProfileImageView User_ProfileImage { get; set; }
    }
}