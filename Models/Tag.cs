using System.ComponentModel.DataAnnotations;

namespace RogueFit.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
