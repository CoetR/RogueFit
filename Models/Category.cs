using System.ComponentModel.DataAnnotations;

namespace RogueFit.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
