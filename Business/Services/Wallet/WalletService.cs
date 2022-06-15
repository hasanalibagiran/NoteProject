using System.Net;
using AutoMapper;
using DataAccess.Services;
using Dto.Common;
using Microsoft.AspNetCore.Http;

namespace Business.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletDal _walletDal;
        private readonly IUserDal _userDal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ContextUser _contextUser;
        private readonly IMapper _mapper;

        public WalletService(IWalletDal walletDal, IUserDal userDal, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _walletDal = walletDal;
            _userDal = userDal;
            _httpContextAccessor = httpContextAccessor;
            _contextUser = _httpContextAccessor.HttpContext.Items["User"] as ContextUser;
            _mapper = mapper;
        }
        

        public async Task<BaseResponseModel> Balance()
        {
            var wallet = await _walletDal.GetAsync(x => x.OwnerUserId == _contextUser.Id);
            if (wallet != null)
            {
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Data = wallet.Balance, Message = "Balance is " + wallet.Balance };
            }
            else
            {
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = false, Message = "User has no wallet" };
            }
        }

        public async Task<BaseResponseModel> DepositMoney(int amount)
        {
            var wallet = await _walletDal.GetAsync(x => x.OwnerUserId == _contextUser.Id);
            if (wallet != null)
            {
                wallet.Balance += amount;
                _walletDal.Update(wallet);
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Deposit completed, new balance is "+ wallet.Balance };
            }
            else
            {
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = false, Message = "User has no wallet" };
            }
        }

        public async Task<BaseResponseModel> TransferMoney(int amount, string targetUserName)
        {
            var userWallet = await _walletDal.GetAsync(x => x.OwnerUserId == _contextUser.Id);
            var targetUser = await _userDal.GetAsync(x => x.UserName == targetUserName);
            if (userWallet != null && targetUser != null)
            {
                if (userWallet.Balance >= amount)
                {
                    userWallet.Balance -= amount;
                    _walletDal.Update(userWallet);
                    var targetUserWallet = await _walletDal.GetAsync(x => x.OwnerUserId == targetUser.Id);
                    targetUserWallet.Balance += amount;
                    _walletDal.Update(targetUserWallet);
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Transfer completed, new balance is " + userWallet.Balance };
                }
                else
                {
                    return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.NotAcceptable, IsSuccess = false, Message = "Not enough money" };
                }
            }
            else
            {
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.BadRequest, IsSuccess = false, Message = "User has no wallet" };
            }
        }

        public async Task<BaseResponseModel> WithdrawMoney(int amount)
        {
            var wallet = await _walletDal.GetAsync(x => x.OwnerUserId == _contextUser.Id);
            if (wallet != null)
            {
                wallet.Balance -= amount;
                _walletDal.Update(wallet);
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = true, Message = "Withdraw completed, new balance is " + wallet.Balance };
            }
            else
            {
                return new BaseResponseModel() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = false, Message = "User has no wallet" };
            }
        }
    }
}