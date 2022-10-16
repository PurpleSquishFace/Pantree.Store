using Pantree.Helpers.Enums;

namespace Pantree.Data.Models.Contracts
{
    public class AccountMain
    {
        public AccountSections Section { get; set; }
        public PantreeUser CurrentUser { get; set; }
        public FriendMasterView FriendMasterView { get; set; }
    }
}