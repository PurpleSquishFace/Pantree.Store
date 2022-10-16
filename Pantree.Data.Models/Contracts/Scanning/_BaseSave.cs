using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pantree.Data.Models.Contracts
{
    public class BaseSave : BaseItem
    {

        public string Notes { get; set; }

        [Display(Name = "Location")]
        public int LocationID { get; set; }
        
        [Display(Name = "Store")]
        public int StoreID { get; set; }
        public int Quantity { get; set; } = 1;

        public SelectList Locations { get; set; }
        public SelectList Stores { get; set; }
    }
}