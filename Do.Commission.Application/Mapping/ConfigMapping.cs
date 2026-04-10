namespace Do.Commission.Application.Mapping;

using Do.Commission.Application.Dtos;
using Do.Commission.Infrastructure.Model;
using Mapster;

public class ConfigMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Infrastructure.Model.Employee, Dtos.EmployeeDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.PositionId, src => src.PositionId)
            .Map(dest => dest.Salary, src => src.Salary)
            .IgnoreNullValues(true);

        TypeAdapterConfig<EmployeeDtoIn, Employee>
        .NewConfig()
        .IgnoreNullValues(true);

    }
}

