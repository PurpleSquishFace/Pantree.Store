using System.ComponentModel.DataAnnotations;

namespace Pantree.Data.Models.Contracts
{
    public class SignUpSubmit
    {
        public string Username { get; set; }
        
        [DataType(DataType.Password)]

        public string Password { get; set; }
        public string Name { get; set; }

        [Display(Name = "Email")]
        public string EmailAddress { get; set; }
        public string? Redirect { get; set; }
    }
}