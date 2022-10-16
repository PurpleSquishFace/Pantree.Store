using System.ComponentModel.DataAnnotations;

namespace Pantree.Data.Models.Contracts
{
    public class SignInSubmit
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? Redirect { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}