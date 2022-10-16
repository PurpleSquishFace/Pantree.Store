using Dapper.Contrib.Extensions;

namespace Pantree.Data.Models.Database
{
    [Table("tbl_Items")]
    public class tbl_Items : IDatabaseTable
    {
        [Key]
        public int ItemID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string IngredientList { get; set; }
        public string ImageUrl { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
