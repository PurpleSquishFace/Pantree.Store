using System.ComponentModel.DataAnnotations;

namespace Pantree.Data.Models.Contracts
{
    public class LocationAdd
    {
        [Display(Name = "Name")]
        [Required]
        public string LocationName { get; set; }

        [Display(Name = "Description")]
        public string LocationDescription { get; set; }
    }
}