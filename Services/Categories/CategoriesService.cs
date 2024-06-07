using moneyManagerBE.Class;
using moneyManagerBE.Data;
using moneyManagerBE.Models;

namespace moneyManagerBE.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly AppDbContext _appDbContext;
        public CategoriesService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public DbResponse<Category> AddCategory(Category category)
        {
            var foundCategoryWithSameName = _appDbContext.Categories
            .Where(theCategory => theCategory.UserId == category.UserId)
            .Where(theCategory => theCategory.Name == category.Name).FirstOrDefault();

            if (foundCategoryWithSameName != null)
            {
                return new DbResponse<Category>
                {
                    IsSuccess = false,
                    Message = "Category with the same name already exists, create another unique category"
                };
            }
            else
            {
                _appDbContext.Categories.Add(category);
                _appDbContext.SaveChanges();

                return new DbResponse<Category>
                {
                    IsSuccess = true,
                    Message = "Created category successful",
                    Data = category
                };
            }
        }

        public DbResponseList<List<Category>> GetAllCategories(int userId, int pageNumber, int pageSize, string search)
        {
            string searchTerm = search.ToLower();

            List<Category> allcategories;
            // if search use searched total, if not then db all count
            int totalCount = 0;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allcategories = _appDbContext.Categories
                .Where(theCategory => theCategory.UserId == userId)
                .Where(theCategory =>
                theCategory.Name.ToLower().Contains(searchTerm) ||
                (theCategory.Description != null && theCategory.Description.ToLower().Contains(searchTerm))
                )
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                totalCount = allcategories.Count();
            }
            else
            {
                allcategories = _appDbContext.Categories
                .Where(theCategory => theCategory.UserId == userId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                totalCount = _appDbContext.Categories.Count();
            }

            DbResponseList<List<Category>> dbResponseList = new DbResponseList<List<Category>>
            {
                Data = allcategories,
                IsSuccess = true,
                Total = totalCount,
                Message = "Success getting categories"
            };

            return dbResponseList;
        }

        public DbResponse<List<string>> DeleteCategory(int id)
        {
            var category = _appDbContext.Categories.FirstOrDefault(category => category.Id == id);

            if (category != null)
            {
                _appDbContext.Categories.Remove(category);
                _appDbContext.SaveChanges();

                return new DbResponse<List<string>>()
                {
                    IsSuccess = true,
                    Message = "Deleted category of " + id
                };
            }
            else
            {
                return new DbResponse<List<string>>()
                {
                    IsSuccess = false,
                    Message = $"Category of {id} does not exist"
                };
            }
        }

        public DbResponse<Category> UpdateCategory(Category category)
        {
            _appDbContext.Categories.Update(category);
            _appDbContext.SaveChanges();

            return new DbResponse<Category>
            {
                IsSuccess = true,
                Message = "Update category successful",
                Data = category
            };
        }

        public DbResponse<Category> GetCategoryById(int categoryId)
        {
            var foundData = _appDbContext.Categories.FirstOrDefault(data => data.Id == categoryId);

            if (foundData != null)
            {
                return new DbResponse<Category>
                {
                    IsSuccess = true,
                    Message = $"Category found",
                    Data = foundData
                };
            }
            else
            {
                return new DbResponse<Category>
                {
                    IsSuccess = false,
                    Message = $"Category of {categoryId} does not exist"
                };
            }
        }
    }
}