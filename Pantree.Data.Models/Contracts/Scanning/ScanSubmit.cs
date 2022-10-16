using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Pantree.Data.Models.Contracts
{
    public class ScanSubmit
    {
        [Required]
        public string Code { get; set; }

        public IFormFile Image { get; set; }
    }
}