namespace Pantree.Data.Models.Contracts
{
    public class FriendMasterView
    {
        public List<FriendView> Friends { get; set; }
        public FriendSearch Search { get; set; }
    }
}