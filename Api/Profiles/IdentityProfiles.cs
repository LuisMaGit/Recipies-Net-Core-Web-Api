using Api.Models.Identity;
using Api.ModelsDto.Requests.Identity;
using AutoMapper;

namespace Api.Profiles
{
    public class IdentityProfiles : Profile
    {
        public IdentityProfiles()
        {
            CreateMap<RegisterDto, Register>();
            CreateMap<LoginDto, Login>();
        }
    }
}