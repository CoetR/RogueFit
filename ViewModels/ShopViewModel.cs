using RogueFit.Models;

namespace RogueFit.ViewModels
{
    public class ShopViewModel
    {
        public List<Category> Categories { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Product> Products { get; set; }
    }
}
