using Dto.Common;
using Dto.Models;
using Entities;

namespace Business.Services
{
    public interface IUserService
    {
        
        
        Task<User> GetUser(AuthModel user);
        Task<BaseResponseModel> SingUpUser(SingUpModel user);
        

       
        

    }
}