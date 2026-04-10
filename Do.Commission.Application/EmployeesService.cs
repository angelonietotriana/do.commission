using Do.Commission.Application.Dtos;
using Do.Commission.Infrastructure.Model;
using Do.Commission.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;

namespace Do.Commission.Application;

public class EmployeesService
{
    private readonly EmployeeRepository _repository;
    private readonly ICommissionsService _commissionsService;
    private readonly IMapper _mapper;

    public EmployeesService(EmployeeRepository repository,
                            ICommissionsService commissionsService,
                            IMapper mapper)
    {
        _repository = repository;
        _commissionsService = commissionsService;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>?> GetAllEmployees()
    {
        List<Employee>? employeesDomain = await _repository.GetEmployeesAsync();

        if (employeesDomain == null)
            return null;

        Domain.Commission.CommissionCalculator calculator = new();

        var employeesDto = employeesDomain
            .Select(employee =>
            {
                var dto = _mapper.Map<EmployeeDto>(employee);
                dto.Commission = calculator.Calculate(employee);
                return dto;
            })
            .ToList();

        return employeesDto;
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var empleyeeDomain = await _repository.GetByIdAsync(id);

        if (empleyeeDomain is not null)
        {
            var employeeDto = _mapper.Map<EmployeeDto>(empleyeeDomain);
            Domain.Commission.CommissionCalculator calculator = new();
            employeeDto.Commission = calculator.Calculate(empleyeeDomain);

            return employeeDto;
        }

        return null;
    }

    public async Task<int> CreateEmployeeAsync(EmployeeDtoIn employeeDtoIn)
    {
        var employeeDomain = _mapper.Map<Employee>(employeeDtoIn);
        return await _repository.Create(employeeDomain);
    }

    public async Task<bool> DeleteEmployeeAsync(int employeeId)
    {
        return await _repository.Remove(employeeId);
    }

    public async Task<int> UpdateEmployeeAsync(int id, EmployeeDtoIn employeeDtoIn)
    {
        var empleyeeDomain = await _repository.GetByIdAsync(id);

        if (empleyeeDomain is null)
            return 0;

        employeeDtoIn.Adapt(empleyeeDomain);

        return await _repository.Update(empleyeeDomain);
    }

}
