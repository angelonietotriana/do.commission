using Do.Commission.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace Do.Commission.Infrastructure.Repositories;

public class EmployeeRepository(MysqlDbContext context) : IEmployeeRepository
{
    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await context.Employees.FirstOrDefaultAsync(employee => employee.Id == id);
    }

    public async Task<List<Employee>?> GetEmployeesAsync()
    {
        return await context.Employees.ToListAsync();
    }

    public async Task<int> Create(Employee employee)
    {
        await context.Employees.AddAsync(employee);
        context.SaveChanges();

        return employee.Id;
    }

    public async Task<int> Update(Employee employee)
    {
        context.Employees.Update(employee);
        context.SaveChanges();

        return employee.Id;
    }

    public async Task<bool> Remove(int id)
    {
        var employee = await context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee != null)
        {
            context.Employees.Remove(employee);
            context.SaveChanges();
        }

        return employee != null;
    }

}