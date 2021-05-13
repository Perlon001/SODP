using AutoMapper;
using SODP.Domain.DTO;
using SODP.Model;

namespace SODP.Domain
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, UserUpdateDTO>();

            CreateMap<Stage, StageDTO>().ReverseMap();

            CreateMap<Project, ProjectDTO>().ReverseMap();
        }
    }
}
