using Dapper.Contrib.Extensions;

namespace Pantree.Data.Models.Database
{
    [Table("tbl_SharedLocations")]
    public class tbl_SharedLocations : IDatabaseTable
    {
        [Key]
        public int SharedID { get; set; }
        public int UserID { get; set; }
        public int LocationID { get; set; }
    }
}
