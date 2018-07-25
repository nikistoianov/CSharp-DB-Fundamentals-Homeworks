using AutoMapper;
using Office.App.Core.DTOs;
using Office.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Office.App.Core
{
    public class OfficeProfile : Profile
    {
        public OfficeProfile()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}
