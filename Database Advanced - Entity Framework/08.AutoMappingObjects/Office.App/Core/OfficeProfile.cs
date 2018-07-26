namespace Office.App.Core
{
    using AutoMapper;
    using DTOs;
    using Models;

    public class OfficeProfile : Profile
    {
        public OfficeProfile()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, ManagerDto>()
                .ForMember(dest => dest.Employees, opt => opt.MapFrom(e => e.ManagerEmployees))
                .ReverseMap();
            //CreateMap<Employee, EmployeeInfoDto>()
            //    .ForMember(dest => dest.ManagerLastName, opt => opt.MapFrom(e => e.Manager.LastName))
            //    .ReverseMap();
        }
    }
}
