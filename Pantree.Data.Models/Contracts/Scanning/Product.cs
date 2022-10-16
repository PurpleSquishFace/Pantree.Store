namespace Pantree.Data.Models.Contracts
{
    public class Product : BaseItem
    {
        public int ProductID { get; set; }
        public int LookupsSinceScan { get; set; }

    }
}