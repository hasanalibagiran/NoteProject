using Dto.Common;
using Dto.Models;

namespace Business.Services
{
    public interface ICategoryService
    {
        //crud operations
        Task<BaseResponseModel> Add(CategoryModel model);
        Task<BaseResponseModel> Update(CategoryModel model);
        Task<BaseResponseModel> Get(int id);
        Task<BaseResponseModel> GetAll();
        Task<BaseResponseModel> Delete(int id);
    }
}