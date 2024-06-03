using AutoMapper;
using AssociationManagement.Application.Dtos.Employees;
using AssociationManagement.Core.Common;
using AssociationManagement.Core.Entities;

namespace AssociationManagement.Application.MappingProfiles {
    public class EmployeeProfile : Profile {
        public EmployeeProfile() {
            CreateMap<EmployeeResponse, Employee>()
                .ReverseMap();

            CreateMap<Employee, EmployeeCreationRequest>()
                .ReverseMap();

            CreateMap<Employee, EmployeeUpdateRequest>()
                .ReverseMap();

            CreateMap<PaginatedList<EmployeeResponse>, PaginatedList<Employee>>()
                .ReverseMap();
        }
    }
}
