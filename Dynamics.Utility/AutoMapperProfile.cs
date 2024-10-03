using AutoMapper;
using Dynamics.Models.Models;
using Dynamics.Models.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dynamics.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<Models.Domain.Region,Models.DTO.RegionDto>

            //map khi thuộc tính có tên khác nhau
            //CreateMap<UserDTO, UserDomain>()
            //    .ForMember(x=>x.Name,opt=>opt.MapFrom(x=>x.FullName));
            CreateMap<Project, UpdateProjectProfileRequestDto>().ReverseMap();

        }
    }
}
