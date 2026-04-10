using Do.Commission.Application.Dtos;
using Do.Commission.Infrastructure.Model;
using Do.Commission.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;

namespace Do.Commission.Application;

public class EmployeesService
{
    private readonly EmployeeRepository _repository;
    private readonly IMapper _mapper;

    public EmployeesService(EmployeeRepository repository,
                            IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los empleados, los transforma al DTO de salida y calcula su comisión.
    /// </summary>
    /// <returns>La lista de empleados lista para ser expuesta por la API.</returns>
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

    /// <summary>
    /// Obtiene los empleados de un departamento que tienen al menos un proyecto y devuelve su información general.
    /// </summary>
    /// <param name="departmentId">Identificador del departamento que se desea consultar.</param>
    /// <returns>La lista de empleados filtrados por departamento y proyecto.</returns>
    public async Task<List<EmployeeDto>> GetEmployeesByDepartmentWithProjectsAsync(int departmentId)
    {
        var employeesDomain = await _repository.GetEmployeesByDepartmentWithProjectsAsync(departmentId);
        Domain.Commission.CommissionCalculator calculator = new();

        return employeesDomain
            .Select(employee =>
            {
                var dto = _mapper.Map<EmployeeDto>(employee);
                dto.Commission = calculator.Calculate(employee);
                return dto;
            })
            .ToList();
    }

    /// <summary>
    /// Obtiene el historial de cargos de los empleados de un departamento que además tienen un proyecto asociado.
    /// </summary>
    /// <param name="departmentId">Identificador del departamento que se desea consultar.</param>
    /// <returns>La lista del historial de cargos filtrado por departamento y proyecto.</returns>
    public async Task<List<EmployeePositionHistoryDto>> GetEmployeePositionHistoriesByDepartmentWithProjectsAsync(int departmentId)
    {
        var employeesDomain = await _repository.GetEmployeesByDepartmentWithProjectsAsync(departmentId);

        return employeesDomain
            .SelectMany(employee => employee.PositionHistories
                .Where(positionHistory => positionHistory.DepartmentId == departmentId && positionHistory.ProjectId != null))
            .Select(positionHistory => _mapper.Map<EmployeePositionHistoryDto>(positionHistory))
            .ToList();
    }

    /// <summary>
    /// Obtiene un empleado por su identificador, lo transforma al DTO de salida y calcula su comisión.
    /// </summary>
    /// <param name="id">Identificador del empleado a consultar.</param>
    /// <returns>El empleado encontrado o <see langword="null"/> si no existe.</returns>
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

    /// <summary>
    /// Crea un empleado nuevo a partir del DTO de entrada y devuelve su identificador.
    /// </summary>
    /// <param name="employeeDtoIn">Datos del empleado que se desea crear.</param>
    /// <returns>El identificador generado para el nuevo empleado.</returns>
    public async Task<int> CreateEmployeeAsync(EmployeeDtoIn employeeDtoIn)
    {
        var employeeDomain = _mapper.Map<Employee>(employeeDtoIn);
        return await _repository.Create(employeeDomain);
    }

    /// <summary>
    /// Elimina un empleado por su identificador.
    /// </summary>
    /// <param name="employeeId">Identificador del empleado que se desea eliminar.</param>
    /// <returns><see langword="true"/> si el empleado fue eliminado; en caso contrario, <see langword="false"/>.</returns>
    public async Task<bool> DeleteEmployeeAsync(int employeeId)
    {
        return await _repository.Remove(employeeId);
    }

    /// <summary>
    /// Actualiza un empleado existente con los nuevos datos enviados en la solicitud.
    /// </summary>
    /// <param name="id">Identificador del empleado que se desea actualizar.</param>
    /// <param name="employeeDtoIn">Datos nuevos que se aplicarán al empleado.</param>
    /// <returns>El identificador del empleado actualizado o <c>0</c> si no existe.</returns>
    public async Task<int> UpdateEmployeeAsync(int id, EmployeeDtoIn employeeDtoIn)
    {
        var empleyeeDomain = await _repository.GetByIdAsync(id);

        if (empleyeeDomain is null)
            return 0;

        employeeDtoIn.Adapt(empleyeeDomain);

        return await _repository.Update(empleyeeDomain);
    }

}
