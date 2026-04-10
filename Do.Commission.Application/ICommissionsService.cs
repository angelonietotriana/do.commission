using Do.Commission.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do.Commission.Application
{
    public  interface ICommissionsService
    {
        decimal CalculateFor(EmployeeDtoIn employeeDto);
    }
}
