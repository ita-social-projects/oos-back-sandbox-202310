using AutoMapper;
using OutOfSchool.Common.Models;

namespace OutOfSchool.AuthCommon.Util;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProviderAdminDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => Constants.PhonePrefix + src.PhoneNumber.Right(Constants.PhoneShortLength)));

        CreateMap<CreateProviderAdminDto, ProviderAdmin>()
            .ForMember(dest => dest.ManagedWorkshops, opt => opt.Ignore());

        CreateMap<CreateMinistryAdminDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => Constants.PhonePrefix + src.PhoneNumber.Right(Constants.PhoneShortLength)));

        CreateMap<CreateMinistryAdminDto, MinistryAdmin>();
        CreateMap<UpdateMinistryAdminDto, MinistryAdmin>();
    }
}