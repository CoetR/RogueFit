using System.ComponentModel.DataAnnotations;

namespace RogueFit.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
