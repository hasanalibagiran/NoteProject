using AutoMapper;
using Business.Services;
using DataAccess.Services;
using Dto.Common;
using Dto.Models;
using Microsoft.AspNetCore.Http;
using Entities;
using System.Net;

namespace Business.Services
{
    public class ArchiveService : IArchiveService
    {

        protected readonly IArchiveDal _archiveDal;
        protected readonly ICategoryDal _categoryDal;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ContextUser _contextUser;

        public ArchiveService(IArchiveDal archiveDal, IMapper mapper, IHttpContextAccessor httpContextAccessor, ICategoryDal categoryDal)
        {
            _archiveDal = archiveDal;
            _categoryDal = categoryDal;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _contextUser = _httpContextAccessor.HttpContext.Items["User"] as ContextUser;
        }

        public async Task<BaseResponseModel> Add(ArchiveModel archiveModel)
        {
            if(archiveModel.IsPublic==true){
                archiveModel.IsForSale = false;
                archiveModel.Price = 0;
            }

            var category = await _categoryDal.GetByIdAsync(archiveModel.CategoryId);
            if (category != null)
            {
                if(category.IsPublic==true){
                    var archive = _mapper.Map<Archive>(archiveModel);
                    archive.OwnerUserId = _contextUser.Id;
                    await _archiveDal.AddAsync(archive);
                    return new BaseResponseModel { StatusCode = (int)HttpStatusCode.OK , IsSuccess = true, Message = "Archive added."};
                }
                else
                {
                    if (category.OwnerUserId == _contextUser.Id){
                        var archive = _mapper.Map<Archive>(archiveModel);
                        archive.OwnerUserId = _contextUser.Id;
                        archive.IsPublic = false;
                        await _archiveDal.AddAsync(archive);
                        return new BaseResponseModel { StatusCode = (int)HttpStatusCode.OK , Message = "Archive added.", IsSuccess = true};
                    }
                    return new BaseResponseModel { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Category is not public.", IsSuccess = false};
                }
            }
            return new BaseResponseModel { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Category not found", IsSuccess = false};
            
            
        }

        public async Task<BaseResponseModel> Delete(int id)
        {
            
            var archive = await _archiveDal.GetAsync(x => x.Id == id && x.OwnerUserId == _contextUser.Id);
            if (archive != null)
            {
                _archiveDal.Delete(archive);
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Archive deleted." };
            }
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Archive not found or permission denied." };

        }

        public async Task<BaseResponseModel> Get(int id)
        {
            var archive = await _archiveDal.GetAsync(x => x.Id == id);
            if (archive != null)
            {
                if(archive.IsPublic == true){
                    var archiveModel = _mapper.Map<ArchiveModel>(archive);
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data = archiveModel };
                }
                else
                {
                    if (archive.OwnerUserId == _contextUser.Id)
                    {
                        var archiveModel = _mapper.Map<ArchiveModel>(archive);
                        return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data = archiveModel };
                    }
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Archive not found or permission denied." };
                }
            }
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Archive not found." };
        }

        public async Task<BaseResponseModel> GetAllArchivesAndCategories()
        {
            var categories = await _categoryDal.GetAllAsync();
            List<Category> availableCategories = new List<Category>();

            foreach (var category in categories)
            {
                if (category.IsPublic == true)
                {
                    availableCategories.Add(category);
                }
                else
                {
                    if (category.OwnerUserId == _contextUser.Id)
                    {
                        availableCategories.Add(category);
                    }
                }
            }   

            List<CategoryAndArchiveModel> categoryAndArchiveModels = new List<CategoryAndArchiveModel>();
            var archives = await _archiveDal.GetAllAsync();
            foreach(var category in availableCategories)
            {
                var categorysArchive = archives.Where(x => x.CategoryId == category.Id).ToList();  
                var archiveList =new List<ArchiveModel>();  
                
                foreach(var archive in categorysArchive)
                {
                    if(archive.IsPublic == true){
                        archiveList.Add(_mapper.Map<ArchiveModel>(archive));
                    }
                    else
                    {
                        if (archive.OwnerUserId == _contextUser.Id)
                        {
                            archiveList.Add(_mapper.Map<ArchiveModel>(archive));
                        }
                    }
                }
                var categoryAndArchiveModel = new CategoryAndArchiveModel() { CategoryName = category.Name, ArchiveList = archiveList };
                categoryAndArchiveModels.Add(categoryAndArchiveModel);

                
            }
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data = categoryAndArchiveModels };

        }

        

        public async Task<BaseResponseModel> GetArchiveListByCategory(int categoryId)
        {
            var category = await _categoryDal.GetAsync(x => x.Id == categoryId);

            if (category != null)
            {
                if(category.IsPublic==true){
                    var archives = await _archiveDal.GetAllAsync(x => x.CategoryId == categoryId);
                    var archiveList = new List<Archive>();
                    foreach(var archive in archives)
                    {
                        if(archive.IsPublic==true){
                            archiveList.Add(archive);
                        }else{
                            if(archive.OwnerUserId==_contextUser.Id){
                                archiveList.Add(archive);
                            }
                        }
                    }
                    var archiveModelList = _mapper.Map<List<ArchiveModel>>(archiveList);
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data = archiveModelList };
                }
                else
                {
                    if (category.OwnerUserId == _contextUser.Id)
                    {
                        var archiveList = await _archiveDal.GetAllAsync(x => x.CategoryId == categoryId);
                        var archiveModelList = _mapper.Map<List<ArchiveModel>>(archiveList);
                        return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data = archiveModelList };
                    }
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Category is not public." };
                }
            }
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Category not found." };


        }

        public async Task<BaseResponseModel> GetAll()
        {
            
            var archives = await _archiveDal.GetAllAsync();
            if (archives != null)
            {
                var archiveList = new List<Archive>();
                foreach(var archive in archives)
                {
                    if(archive.IsPublic == true){
                        archiveList.Add(archive);
                    }
                    else
                    {
                        if (archive.OwnerUserId == _contextUser.Id)
                        {
                            archiveList.Add(archive);
                        }
                    }
                }
                var archiveListModel = _mapper.Map<List<ArchiveModel>>(archiveList);
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data = archiveListModel };
            }
            
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Archives not found." } ;

        }

        public async Task<BaseResponseModel> Update(ArchiveModel archiveModel)
        {
            
            var archive = await _archiveDal.GetAsync(x => x.Id == archiveModel.Id && x.OwnerUserId == _contextUser.Id);
            if(archive != null)
            {
                var category = await _categoryDal.GetByIdAsync(archiveModel.CategoryId);
                if (category.IsPublic==true){
                    if(archiveModel.IsPublic==false && archiveModel.IsForSale==true){
                        archive.IsForSale = archiveModel.IsForSale;
                        archive.Price = archiveModel.Price;
                    }
                    archive.Title = archiveModel.Title;
                    archive.Note = archiveModel.Note;        
                    archive.CategoryId = archiveModel.CategoryId;
                    archive.IsPublic = archiveModel.IsPublic;
                    _archiveDal.Update(archive);
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Archive updated." };
                }else{
                    if (category.OwnerUserId == _contextUser.Id){
                        if(archiveModel.IsPublic==false && archiveModel.IsForSale==true){
                            archive.IsForSale = archiveModel.IsForSale;
                            archive.Price = archiveModel.Price;
                        }
                        archive.Title = archiveModel.Title;
                        archive.Note = archiveModel.Note;        
                        archive.CategoryId = archiveModel.CategoryId;
                        archive.IsPublic = archiveModel.IsPublic;
                        _archiveDal.Update(archive);
                        return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Archive updated." };
                    }
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Category is not public." };
                }
                
            }
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "Archive not found." };
            
    }


}
}