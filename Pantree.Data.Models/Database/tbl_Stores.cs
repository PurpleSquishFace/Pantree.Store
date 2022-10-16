using Dapper.Contrib.Extensions;

namespace Pantree.Data.Models.Database
{
    [Table("tbl_Stores")]
    public class tbl_Stores : IDatabaseTable
    {
        [Key]
        public int StoreID { get; set; }
        public int UserID { get; set; }
        public int LocationID { get; set; }
        public string StoreName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
