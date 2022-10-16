using Dapper.Contrib.Extensions;

namespace Pantree.Data.Models.Database
{
    [Table("tbl_UserFriends")]
    public class tbl_UserFriends : IDatabaseTable
    {
        [Key]
        public int UserFriendID { get; set; }
        public int UserID_Requester { get; set; }
        public int UserID_Addressee { get; set; }
        public int FriendStatusID { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime? DateAccepted { get; set; }
    }
}