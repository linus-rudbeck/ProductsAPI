using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsAPI.Models
{
    public class Product
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public decimal Price { get; set; }


        [ForeignKey("User"), ValidateNever]
        public string? UserId { get; set; }

        [ValidateNever]
        public CustomUser? User { get; set; }
    }
}
