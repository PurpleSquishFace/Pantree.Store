using Dapper.Contrib.Extensions;

namespace Pantree.Data.Models.Database
{
    [Table("tbl_Users")]
    public class tbl_Users : IDatabaseTable
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
    }
}