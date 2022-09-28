using AutoMapper;
using MagicVilla.VillaAPI.VillaAPI.Models;
using MagicVilla.VillaAPI.VillaAPI.Models.Dto;

namespace MagicVilla.VillaAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        }
    }
}
