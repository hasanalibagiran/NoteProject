using DataAccess.Repositories;
using Entities;

namespace DataAccess.Services
{
    public class WalletDal:GenericRepository<Wallet>,IWalletDal
    {
        public WalletDal(AppDbContext context) : base(context)
        {
        }
    }
   
}