using System.Net;
using AutoMapper;
using DataAccess.Services;
using Dto.Common;
using Dto.Models;
using Entities;
using Microsoft.AspNetCore.Http;

namespace Business.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryDal _categoryDal;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ContextUser _contextUser;

        public CategoryService(ICategoryDal categoryDal, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _categoryDal = categoryDal;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _contextUser = _httpContextAccessor.HttpContext.Items["User"] as ContextUser;
        }
        

        public async Task<BaseResponseModel> Add(CategoryModel categoryModel)
        {
            if(categoryModel.IsPublic==true){
                categoryModel.IsForSale = false;
                categoryModel.Price = 0;
            }

            var category = await _categoryDal.GetAsync(x => x.Name == categoryModel.Name);
            if (category == null){
                var categoryEntity = _mapper.Map<Category>(categoryModel);
                categoryEntity.OwnerUserId = _contextUser.Id;
                await _categoryDal.AddAsync(categoryEntity);
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Category added." };
            }
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Category already exists." };


        }

        public async Task<BaseResponseModel> Delete(int id)
        {
            var category = await _categoryDal.GetAsync(x => x.Id == id && x.OwnerUserId == _contextUser.Id);
            if (category != null)
            {
                _categoryDal.Delete(category);
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Category deleted." };
            }
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Category not found or permission denied." };
            
        }

        public async Task<BaseResponseModel> Get(int id)
        {
            var category = await _categoryDal.GetAsync(x => x.Id == id);
            if (category != null)
            {
                if(category.IsPublic==true){
                    var categoryModel = _mapper.Map<CategoryModel>(category);
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data = categoryModel };
                }else if(category.OwnerUserId==_contextUser.Id){
                    var categoryModel = _mapper.Map<CategoryModel>(category);
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data = categoryModel };
                }else{
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Category is not public." };
                }

                
            }
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Category not found." };
        }

        public async Task<BaseResponseModel> GetAll()
        {
            var categories = await _categoryDal.GetAllAsync();
            if (categories != null)
            {
                var  categoriesList = new List<Category>();
                foreach (var category in categories)
                {
                    if(category.IsPublic==true){
                        categoriesList.Add(category);
                    }else if(category.OwnerUserId==_contextUser.Id){
                        categoriesList.Add(category);
                    }
                }
                var categoriesModel = _mapper.Map<List<CategoryModel>>(categoriesList);
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data = categoriesModel };
            }
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Categories not found." };
        }

        public async Task<BaseResponseModel> Update(CategoryModel categoryModel)
        {
            var category = await _categoryDal.GetAsync(x => x.Id == categoryModel.Id);
            if (category != null)
            {
                if(category.IsPublic==true){
                    if(categoryModel.IsPublic==false && categoryModel.IsForSale==true){
                            category.IsForSale = categoryModel.IsForSale;
                            category.Price = categoryModel.Price;
                        }
                    category.Name = categoryModel.Name;
                    _categoryDal.Update(category);
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Category updated." };
                }else if (category.OwnerUserId == _contextUser.Id){
                    if(categoryModel.IsPublic==false && categoryModel.IsForSale==true){
                            category.IsForSale = categoryModel.IsForSale;
                            category.Price = categoryModel.Price;
                        }
                    category.Name = categoryModel.Name;
                    _categoryDal.Update(category);
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Category updated." };
                }else {
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Category is not public." };
                }
            }
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Category not found." };

        }
    }
}