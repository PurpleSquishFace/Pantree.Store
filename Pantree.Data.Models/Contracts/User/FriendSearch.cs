namespace Pantree.Data.Models.Contracts
{
    public class FriendSearch
    {
        public string SearchQuery { get; set; }

        public List<UserSearchResult> Results { get; set; }
    }
}