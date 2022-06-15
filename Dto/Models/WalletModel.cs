using Dto.Common;

namespace Dto.Models
{
    public class WalletModel:BaseModel
    {
        
        public int Balance { get; set; }
        public int OwnerUserId { get; set; }
        

    }
}