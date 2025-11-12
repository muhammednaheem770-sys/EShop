using System.ComponentModel.DataAnnotations;

namespace EShop.Dto.CategoryModel
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage ="Category name is required.")]
        [StringLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; } 
    }
}
