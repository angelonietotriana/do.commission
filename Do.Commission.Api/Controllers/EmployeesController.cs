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

        // GET: api/employees
        [Authorize(Roles = AuthRoles.AdministradorOUsuario)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        {
            var employees = await _employeesService.GetAllEmployees(); 
            return Ok(employees);
        }

        // GET: api/employees/{id}
        [Authorize(Roles = AuthRoles.AdministradorOUsuario)]
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById(int id)
        {
            var employeeDomain = await _employeesService.GetEmployeeByIdAsync(id); 

            if (employeeDomain is null)
                return NotFound();

            return Ok(employeeDomain);
        }

        // POST: api/employees
        [Authorize(Roles = AuthRoles.Administrador)]
        [HttpPost]
        public async Task<ActionResult<EmployeeDtoIn>> CreateAsync([FromBody] EmployeeDtoIn employeeDtoIn)
        {
            if (employeeDtoIn == null)
                return BadRequest();

            var created = await _employeesService.CreateEmployeeAsync(employeeDtoIn);

            return CreatedAtAction(nameof(GetById), new { id = created }, employeeDtoIn);
        }

        // PUT: api/employees/{id}
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

        // DELETE: api/employees/{id}
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
