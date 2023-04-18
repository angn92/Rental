using AdministartionConsole.Models.Dto;
using Rental.Core.Domain;

namespace AdministartionConsole.Helpers
{
    public interface ICategoryDtoHelper 
    {
        List<CategoryDto> PrepareResult(List<Category> categories);
    }

    public class CategoryDtoHelper : ICategoryDtoHelper
    {
        public List<CategoryDto> PrepareResult(List<Category> categories)
        {
            var categoryResult = new List<CategoryDto>();

            foreach (var item in categories)
            {
                categoryResult.Add(new CategoryDto
                {
                    Name = item.Name
                });
            }

            return categoryResult;
        }
    }
}
