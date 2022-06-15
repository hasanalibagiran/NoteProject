using DataAccess.Services;
using Dto.Models;
using Entities;
using System.Net;
using Microsoft.AspNetCore.Http;
using Dto.Common;
using AutoMapper;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ContextUser _contextUser;
        protected readonly IWalletDal _walletDal;
        protected readonly IMapper _mapper;


        public UserService(IUserDal userDal, IHttpContextAccessor httpContextAccessor,IMapper mapper, IWalletDal walletDal)
        {
            _httpContextAccessor = httpContextAccessor;
            _userDal = userDal;
            _contextUser = _httpContextAccessor.HttpContext.Items["User"] as ContextUser;
            _mapper = mapper;
            _walletDal = walletDal;
            
        }

        public async Task<User> GetUser(AuthModel user)
        {
            var loginUser = await _userDal.GetAsync(x => x.UserName == user.UserName && x.Password == user.Password);
            if (loginUser != null)
            {
                return loginUser;
            }
            else
            {
                return null;
            }
        }

        public async Task<BaseResponseModel> SingUpUser(SingUpModel user)
        {
            var userNameControl = await _userDal.GetAsync(x => x.UserName == user.UserName);

            if(userNameControl == null){
                var singUpUser = _mapper.Map<User>(user);   
                await _userDal.AddAsync(singUpUser);
                var newUser = await _userDal.GetAsync(x => x.UserName == singUpUser.UserName);
                var userWalletModel = new WalletModel
                {
                    OwnerUserId = newUser.Id,
                    Balance = 0
                };

                var userWallet = _mapper.Map<Wallet>(userWalletModel);

                await _walletDal.AddAsync(userWallet);
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "User registration completed. " };
            }
            return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "User name already exists." };
            
            
        }

        
    }
}