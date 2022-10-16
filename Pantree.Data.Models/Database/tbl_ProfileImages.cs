using Dapper.Contrib.Extensions;

namespace Pantree.Data.Models.Database
{
    [Table("tbl_ProfileImages")]
    public class tbl_ProfileImages : IDatabaseTable
    {
        [Key]
        public int ProfileImageID { get; set; }
        public int UserID { get; set; }
        public byte[] ProfileImage { get; set; }
        public string AlternativeText { get; set; }
    }
}
