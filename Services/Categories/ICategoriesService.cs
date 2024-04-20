using moneyManagerBE.Class;
using moneyManagerBE.Models;

namespace moneyManagerBE.Services.Categories
{
    public interface ICategoriesServices
    {
        DbResponse<Category> AddCategory(Category category);
        DbResponseList<List<Category>> GetAllCategories(int userId, int pageNumber, int pageSize, string search);

        DbResponse<List<string>> DeleteCategory(int id);

        DbResponse<Category> UpdateCategory(Category category);
        DbResponse<Category> GetCategoryById(int categoryId);
    }
}