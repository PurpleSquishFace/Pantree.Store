using Dapper.Contrib.Extensions;

namespace Pantree.Data.Models.Database
{
    [Table("tbl_Locations")]
    public class tbl_Locations : IDatabaseTable
    {
        [Key]
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
