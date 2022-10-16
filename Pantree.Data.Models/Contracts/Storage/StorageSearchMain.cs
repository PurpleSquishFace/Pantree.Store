namespace Pantree.Data.Models.Contracts
{
    public class StorageSearchMain
    {
        public string SearchQuery { get; set; }
        public List<StorageSearch> Results { get; set; }
    }
}