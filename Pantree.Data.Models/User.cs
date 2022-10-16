using Pantree.Data.Models.Contracts;

namespace Pantree.Data.Models
{
    public class User
    {
        public int UserID { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public bool RememberMe { get; set; }
        public PantreeUser UserDetails { get; set; }
    }
}