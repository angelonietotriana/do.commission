using Do.Commission.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do.Commission.Application;

public interface IEmployeesService
{
    Task<Employee?> GetEmployeeByIdAsync(int id);
}
