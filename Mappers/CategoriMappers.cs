using App.DTO.Category;
using App.Models;

namespace App.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryDTO ToCategoriDto(this Category categoryModel)
        {
            return new CategoryDTO
            {
                CategoryId = categoryModel.CategoryId,
                Name = categoryModel.Name
            };
        }
        public static Category ToFromCreateDOT(this CreateCategoryRequest category)
        {
            return new Category
            {
                Name = category.Name
            };
        }
    }
}

