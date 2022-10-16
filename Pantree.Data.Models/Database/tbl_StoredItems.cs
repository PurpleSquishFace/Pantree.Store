using Dapper.Contrib.Extensions;

namespace Pantree.Data.Models.Database
{
    [Table("tbl_StoredItems")]
    public class tbl_StoredItems : IDatabaseTable
    {
        [ExplicitKey]
        public int StoreID { get; set; }
        [ExplicitKey]
        public int ItemID { get; set; }
        public int Quantity { get; set; }
    }
}
