using AutoMapper;
using WebAdvert.AdvertApi.Dto;
using WebAdvert.Web.Models.Adverts;
using WebAdvert.Web.ServiceClients.Models;

namespace WebAdvert.Web.ServiceClients.Mappings
{
    public class AdvertMappings : Profile
    {
        public AdvertMappings()
        {
            CreateMap<CreateAdvertModel, AdvertModel>();
            CreateMap<AdvertModel, AdvertDto>();
            CreateMap<CreateAdvertResponseDto, CreateAdvertResponseModel>();
            CreateMap<ConfirmAdvertModel, ConfirmAdvertDto>();
        }
    }
}
