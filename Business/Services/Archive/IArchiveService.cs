using Dto.Common;
using Dto.Models;

namespace Business.Services
{
    public interface IArchiveService
    {
        //crud operations
        Task<BaseResponseModel> Add(ArchiveModel model);
        Task<BaseResponseModel> Update(ArchiveModel model);
        Task<BaseResponseModel> Get(int id);
        Task<BaseResponseModel> GetAll();
        Task<BaseResponseModel> Delete(int id);
        Task<BaseResponseModel> GetArchiveListByCategory(int categoryId);
        Task<BaseResponseModel> GetAllArchivesAndCategories();

    }
}