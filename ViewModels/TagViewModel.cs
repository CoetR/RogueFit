using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RogueFit.ViewModels
{
    public class TagViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int SelectedSubCategoryId { get; set; }

        public IEnumerable<SelectListItem> SubCategories { get; set; }
    }
}
