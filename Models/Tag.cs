using System.ComponentModel.DataAnnotations;

namespace RogueFit.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }

        public int SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
