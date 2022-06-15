using Dto.Common;

namespace Business.Services
{
    public interface IWalletService
    {
        Task<BaseResponseModel> Balance();
        Task<BaseResponseModel> DepositMoney( int amount);
        Task<BaseResponseModel> WithdrawMoney(int amount);
        Task<BaseResponseModel> TransferMoney(int amount, string targetUserName);

    }
}