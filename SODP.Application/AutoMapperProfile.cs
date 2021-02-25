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

            CreateMap<Stage, StageDTO>();
            CreateMap<StageCreateDTO, Stage>();
            CreateMap<StageUpdateDTO, Stage>();

            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectCreateDTO,Project>();
            CreateMap<ProjectUpdateDTO,Project>();
        }
    }
}
