using Do.Commission.Application.Dtos;
using Do.Commission.Domain.Commission;
using Do.Commission.Infrastructure.Model;
using MapsterMapper;

namespace Do.Commission.Application;

public class CommissionsService : ICommissionsService
{
    private readonly ICommissionCalculator _calculator;
    private readonly IMapper _mapper;

    public CommissionsService(ICommissionCalculator calculator, IMapper mapper)
    {
        _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    // Calcula la comisión de un empleado
    public decimal CalculateFor(EmployeeDtoIn employeeDto)
    {
        Employee employeeObj = _mapper.Map<Employee>(employeeDto);
        return _calculator.Calculate(employeeObj);
    }

    // Calcula comisiones para varios empleados y devuelve pares (empleado, comisión)
    public IEnumerable<(Employee Employee, decimal Commission)> CalculateForAll(IEnumerable<Employee> employees) =>
        employees.Select(e => (e, _calculator.Calculate(e)));
}


