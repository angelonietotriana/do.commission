namespace Do.Commission.Application.Mapping;

using Do.Commission.Application.Dtos;
using Do.Commission.Infrastructure.Model;
using Mapster;

public class ConfigMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PositionHistory, EmployeePositionHistoryDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.EmployeeId, src => src.EmployeeId)
            .Map(dest => dest.PositionId, src => src.PositionId)
            .Map(dest => dest.DepartmentId, src => src.DepartmentId)
            .Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.Name : null)
            .Map(dest => dest.ProjectId, src => src.ProjectId)
            .Map(dest => dest.ProjectName, src => src.Projects != null ? src.Projects.Name : null)
            .Map(dest => dest.StartDate, src => src.StartDate)
            .Map(dest => dest.EndDate, src => src.EndDate)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.ReasonChange, src => src.ReasonChange)
            .IgnoreNullValues(true);

        config.NewConfig<Infrastructure.Model.Employee, Dtos.EmployeeDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.PositionId, src => src.PositionId)
            .Map(dest => dest.Salary, src => src.Salary)
            .Map(dest => dest.PositionHistories, src => src.PositionHistories)
            .IgnoreNullValues(true);

        TypeAdapterConfig<EmployeeDtoIn, Employee>
        .NewConfig()
        .IgnoreNullValues(true);

    }
}

