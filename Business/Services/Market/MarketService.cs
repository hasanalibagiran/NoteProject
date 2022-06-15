using System.Net;
using AutoMapper;
using DataAccess.Services;
using Dto.Common;
using Dto.Models;
using Microsoft.AspNetCore.Http;

namespace Business.Services.Market
{
    public class MarketService : IMarketService
    {

        private readonly IArchiveDal _archiveDal;
        private readonly ICategoryDal _categoryDal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ContextUser _contextUser;
        private readonly IMapper _mapper;
        private readonly IWalletService _walletService;
        private readonly IUserDal _userDal;
        public MarketService(IArchiveDal archiveDal, ICategoryDal categoryDal, IHttpContextAccessor httpContextAccessor, IMapper mapper, IWalletService walletService,IUserDal userDal)
        {
            _archiveDal = archiveDal;
            _categoryDal = categoryDal;
            _httpContextAccessor = httpContextAccessor;
            _contextUser = _httpContextAccessor.HttpContext.Items["User"] as ContextUser;
            _mapper = mapper;
            _walletService = walletService;
            _userDal = userDal;
        }


        public async Task<BaseResponseModel> ArchivesForSale()
        {   
            var archives = await _archiveDal.GetAllAsync(x=> x.IsForSale == true);
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data=archives ,Message = "Archives for sale" };
        }

        public async Task<BaseResponseModel> BuyArchive(int archiveId)
        {
            var archive = await _archiveDal.GetAsync(x => x.Id == archiveId);
            if(archive.IsForSale == true){
                
                var targetUser = await _userDal.GetAsync(x => x.Id == archive.OwnerUserId);
                var transfer = await _walletService.TransferMoney(archive.Price, targetUser.UserName);
                if (transfer.Message != "Not enough money"){
                    
                    archive.IsForSale = false;
                    archive.OwnerUserId = _contextUser.Id;
                    _archiveDal.Update(archive);
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Archive bought" };
                }
                else{

                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = false, Message = "Not enough money" };

                } 
                
            }
            else{
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = false, Message = "Archive is not for sale" };
            }
        }

        public async Task<BaseResponseModel> BuyCategory(int categoryId)
        {
            var category = await _categoryDal.GetAsync(x => x.Id == categoryId);
            if(category.IsForSale == true){
                
                var targetUser = await _userDal.GetAsync(x => x.Id == category.OwnerUserId);
                var transfer = await _walletService.TransferMoney(category.Price, targetUser.UserName);
                if (transfer.Message != "Not enough money"){
                    
                    category.IsForSale = false;
                    category.OwnerUserId = _contextUser.Id;
                    _categoryDal.Update(category);

                    var archives = await _archiveDal.GetAllAsync(x => x.CategoryId == category.Id);
                    foreach (var archive in archives)
                    {
                        archive.IsForSale = false;
                        archive.OwnerUserId = _contextUser.Id;
                        _archiveDal.Update(archive);
                    }

                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Category and category's archives bought" };
                }
                else{

                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = false, Message = "Not enough money" };

                }

                
                
            }
            else{
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = false, Message = "Category is not for sale" };
        }}

        public async Task<BaseResponseModel> CategoriesForSale()
        {
            var categories = await _categoryDal.GetAllAsync(x => x.IsForSale == true);
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data = categories, Message = "Categories for sale" };
        }

        public async Task<BaseResponseModel> SellArchive(int archiveId,int price)
        {
            var archive = await _archiveDal.GetAsync(x => x.Id == archiveId);
            if (archive != null && archive.IsForSale == false && archive.OwnerUserId == _contextUser.Id)
            {
                archive.IsForSale = true;
                archive.Price = price;
                _archiveDal.Update(archive);
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Archive is now for sale" };
            }
            else
            {
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = false, Message = "Archive not found or already for sale or not yours" };
            }
        }

        public async Task<BaseResponseModel> SellCategory(int categoryId, int price)
        {
            var category = await _categoryDal.GetAsync(x => x.Id == categoryId);
            if (category != null && category.IsForSale == false && category.OwnerUserId == _contextUser.Id)
            {
                category.IsForSale = true;
                category.Price = price;
                _categoryDal.Update(category);
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Category is now for sale" };
            }
            else
            {
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = false, Message = "Category not found or already for sale or not yours" };
            }
        }

        public async Task<BaseResponseModel> ShowMyItemsForSale()
        {
            var categories = await _categoryDal.GetAllAsync(x => x.OwnerUserId == _contextUser.Id && x.IsForSale == true);
            var categoryList = _mapper.Map<List<CategoryModel>>(categories);

            var archives = await _archiveDal.GetAllAsync(x => x.OwnerUserId == _contextUser.Id && x.IsForSale == true);
            var archiveList = _mapper.Map<List<ArchiveModel>>(archives);

            var itemsForSale = new ItemsForSaleModel() { Title = "My items for sale", CategoryList = categoryList, ArchiveList = archiveList };

            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data = itemsForSale, Message = "My items for sale" };
        }
    }

        
    }