using Do.Commission.Application;
using Do.Commission.Application.Dtos;
using Do.Commission_.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do.Commission_.Controllers
{
    [Route("api/employees/departments")]
    [ApiController]
    public class DepartmentController(EmployeesService employeesService) : ControllerBase
    {
        private readonly EmployeesService _employeesService = employeesService;

        /// <summary>
        /// Obtiene el historial de cargos de los empleados que pertenecen a un departamento específico y que tienen al menos un proyecto asignado.
        /// </summary>
        /// <param name="departmentId">Identificador del departamento que se desea consultar.</param>
        /// <returns>
        /// Un resultado <see cref="BadRequestResult"/> si el identificador no es válido,
        /// o <see cref="OkObjectResult"/> con el historial encontrado.
        /// </returns>
        [Authorize(Roles = AuthRoles.AdministradorOUsuario)]
        [HttpGet("{departmentId}/with-projects")]
        public async Task<ActionResult<IEnumerable<EmployeePositionHistoryDto>>> GetEmployeesWithProjectsByDepartment(int departmentId)
        {
            if (departmentId <= 0)
                return BadRequest();

            var employeePositionHistories = await _employeesService.GetEmployeePositionHistoriesByDepartmentWithProjectsAsync(departmentId);
            return Ok(employeePositionHistories);
        }
    }
}
