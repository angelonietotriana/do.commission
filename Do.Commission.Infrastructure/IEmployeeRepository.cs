using Do.Commission.Infrastructure.Model;

namespace Do.Commission.Infrastructure;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id);
}
