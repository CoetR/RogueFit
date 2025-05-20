using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RogueFit.ViewModels
{
    public class SubCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int SelectedCategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
