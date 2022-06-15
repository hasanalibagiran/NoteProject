using Dto.Common;

namespace Business.Services.Market
{
    public interface IMarketService
    {
        Task<BaseResponseModel> ArchivesForSale();
        Task<BaseResponseModel> CategoriesForSale();
        Task<BaseResponseModel> BuyArchive(int archiveId);
        Task<BaseResponseModel> BuyCategory(int categoryId);
        Task<BaseResponseModel> SellArchive(int archiveId, int price);
        Task<BaseResponseModel> SellCategory(int categoryId, int price);
        Task<BaseResponseModel> ShowMyItemsForSale();


    }
}