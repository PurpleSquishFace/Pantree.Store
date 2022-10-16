using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pantree.Data.Models.Contracts
{
    public class StoreSelect
    {
        public string ElementID { get; set; }
        public SelectList Stores { get; set; }

        [Display(Name = "Store")]
        public int StoreID { get; set; }
    }
}