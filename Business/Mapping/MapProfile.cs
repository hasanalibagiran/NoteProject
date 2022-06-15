using AutoMapper;
using Dto.Models;
using Entities;

namespace Business.Mapping
{
    public class MapProfile:Profile
    {
        
        public MapProfile()
        {   

            CreateMap<AuthModel,User>();
            CreateMap<ArchiveModel,Archive>().ReverseMap();
            CreateMap<SingUpModel,User>();
            CreateMap<CategoryModel,Category>().ReverseMap();
            CreateMap<WalletModel,Wallet>().ReverseMap();
            
        
        }
    }
}