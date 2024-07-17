namespace IdentityServerAspNetIdentity.Profile;
using AutoMapper;
using IdentityServerAspNetIdentity.Models;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Pages.Account.Registration.InputModel, ApplicationUser>();
    }
}
