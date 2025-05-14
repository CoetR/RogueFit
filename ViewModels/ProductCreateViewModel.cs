using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RogueFit.Models;
using System.ComponentModel.DataAnnotations;

namespace RogueFit.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public IFormFile ImageFile { get; set; }


        public int? SelectedCategoryId { get; set; }
        public int? SelectedSubCategoryId { get; set; }
        public int? SelectedTagId { get; set; }
        
        public string NewCategoryName { get; set; }
        public string NewSubCategoryName { get; set; }
        public string NewTagName { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> SubCategories { get; set; } = new();
        public List<SelectListItem> Tags { get; set; } = new();



    }
}
