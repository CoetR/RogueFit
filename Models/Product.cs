using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RogueFit.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        [Precision(18, 2)]
        public required Decimal Price { get; set; }
        public required string Image {  get; set; }
    }
}
