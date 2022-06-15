using Dto.Models;
using Entities;

namespace Business.Services.Token
{
    public interface ITokenHandler
    {
        public TokenModel CreateToken(User user);
    }
}