using System.ComponentModel.DataAnnotations;

namespace RogueFit.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
