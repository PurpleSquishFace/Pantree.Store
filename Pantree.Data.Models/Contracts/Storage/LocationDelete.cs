using System.ComponentModel.DataAnnotations;

namespace Pantree.Data.Models.Contracts
{
    public class LocationDelete
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        [Display(Name = "Name")]
        public string EnteredName { get; set; }
        public bool InputMatch => LocationName == EnteredName;
    }
}