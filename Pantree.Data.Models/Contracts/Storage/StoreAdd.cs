using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pantree.Data.Models.Contracts
{
    public class StoreAdd
    {
        [Display(Name = "Location")]
        [Required]
        public int LocationID { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string StoreName { get; set; }

        [Display(Name = "Description")]
        public string StoreDescription { get; set; }

        public SelectList Locations { get; set; }
    }
}