using Do.Commission.Application;
using Do.Commission.Application.Dtos;
using Do.Commission_.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do.Commission_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeesService _employeesService;

        public EmployeesController(EmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        /// <summary>
        /// Obtiene la lista completa de empleados con su información principal y su historial cuando exista.
        /// </summary>
        /// <returns>Un resultado <see cref="OkObjectResult"/> con la colección de empleados.</returns>
        [Authorize(Roles = AuthRoles.AdministradorOUsuario)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        {
            var employees = await _employeesService.GetAllEmployees();
            return Ok(employees);
        }

        /// <summary>
        /// Obtiene un empleado por su identificador, incluyendo su historial cuando existe información relacionada.
        /// </summary>
        /// <param name="id">Identificador del empleado a consultar.</param>
        /// <returns>
        /// Un resultado <see cref="NotFoundResult"/> si el empleado no existe,
        /// o <see cref="OkObjectResult"/> con la información del empleado.
        /// </returns>
        [Authorize(Roles = AuthRoles.AdministradorOUsuario)]
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById(int id)
        {
            var employeeDomain = await _employeesService.GetEmployeeByIdAsync(id);

            if (employeeDomain is null)
                return NotFound();

            return Ok(employeeDomain);
        }

        /// <summary>
        /// Crea un nuevo empleado a partir de los datos enviados en la solicitud.
        /// </summary>
        /// <param name="employeeDtoIn">Datos necesarios para crear el empleado.</param>
        /// <returns>
        /// Un resultado <see cref="BadRequestResult"/> si la solicitud no contiene datos,
        /// o <see cref="CreatedAtActionResult"/> con la referencia al recurso creado.
        /// </returns>
        [Authorize(Roles = AuthRoles.Administrador)]
        [HttpPost]
        public async Task<ActionResult<EmployeeDtoIn>> CreateAsync([FromBody] EmployeeDtoIn employeeDtoIn)
        {
            if (employeeDtoIn == null)
                return BadRequest();

            var created = await _employeesService.CreateEmployeeAsync(employeeDtoIn);

            return CreatedAtAction(nameof(GetById), new { id = created }, employeeDtoIn);
        }

        /// <summary>
        /// Actualiza la información de un empleado existente.
        /// </summary>
        /// <param name="id">Identificador del empleado que se desea actualizar.</param>
        /// <param name="updated">Datos nuevos que se aplicarán al empleado.</param>
        /// <returns>
        /// Un resultado <see cref="BadRequestResult"/> si no se envían datos,
        /// <see cref="NotFoundResult"/> si el empleado no existe,
        /// o <see cref="OkObjectResult"/> con el identificador actualizado.
        /// </returns>
        [Authorize(Roles = AuthRoles.Administrador)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeDtoIn updated)
        {
            if (updated == null)
                return BadRequest();

            int result = await _employeesService.UpdateEmployeeAsync(id, updated);

            if (result == decimal.Zero)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Elimina un empleado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del empleado que se desea eliminar.</param>
        /// <returns>
        /// Un resultado <see cref="NotFoundResult"/> si el empleado no existe,
        /// o <see cref="OkObjectResult"/> con el resultado de la operación.
        /// </returns>
        [Authorize(Roles = AuthRoles.Administrador)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _employeesService.DeleteEmployeeAsync(id);

            if (!result)
                return NotFound();

            return Ok(result);
        }
    }
}
