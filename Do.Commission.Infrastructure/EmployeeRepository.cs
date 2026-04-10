using Do.Commission.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace Do.Commission.Infrastructure.Repositories;

/// <summary>
/// Repositorio encargado de consultar y persistir información de empleados en la base de datos.
/// Se realiza carga explícita de relaciones en las consultas principales porque la API expone
/// el historial del empleado junto con el departamento y el proyecto asociados a cada movimiento.
/// </summary>
public class EmployeeRepository(MysqlDbContext context) : IEmployeeRepository
{
    /// <summary>
    /// Obtiene un empleado por su identificador e incluye su historial de posiciones.
    /// También carga el departamento y el proyecto de cada registro del historial para que
    /// la capa de aplicación pueda construir el DTO completo sin realizar consultas adicionales.
    /// </summary>
    /// <param name="id">Identificador del empleado que se desea consultar.</param>
    /// <returns>El empleado encontrado con sus relaciones cargadas o <see langword="null"/> si no existe.</returns>
    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await context.Employees
            .Include(employee => employee.PositionHistories)
                .ThenInclude(positionHistory => positionHistory.Department)
            .Include(employee => employee.PositionHistories)
                .ThenInclude(positionHistory => positionHistory.Projects)
            .FirstOrDefaultAsync(employee => employee.Id == id);
    }

    /// <summary>
    /// Obtiene todos los empleados e incluye su historial de posiciones.
    /// La consulta trae además el departamento y el proyecto relacionados a cada movimiento,
    /// porque el resultado final debe poder mostrar el historial completo en el DTO de salida.
    /// </summary>
    /// <returns>La lista de empleados con sus relaciones cargadas.</returns>
    public async Task<List<Employee>?> GetEmployeesAsync()
    {
        return await context.Employees
            .Include(employee => employee.PositionHistories)
                .ThenInclude(positionHistory => positionHistory.Department)
            .Include(employee => employee.PositionHistories)
                .ThenInclude(positionHistory => positionHistory.Projects)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene los empleados que pertenecen a un departamento específico y que tienen al menos un proyecto asociado.
    /// Se incluyen las relaciones del historial porque el filtro se basa en <see cref="Employee.PositionHistories"/>
    /// y porque el endpoint que consume esta consulta necesita devolver el detalle del historial ya resuelto,
    /// incluyendo nombres de departamento y proyecto, sin depender de nuevas consultas posteriores.
    /// </summary>
    /// <param name="departmentId">Identificador del departamento que se desea filtrar.</param>
    /// <returns>La lista de empleados que cumplen la condición indicada.</returns>
    public async Task<List<Employee>> GetEmployeesByDepartmentWithProjectsAsync(int departmentId)
    {
        return await context.Employees
            .Include(employee => employee.PositionHistories)
                .ThenInclude(positionHistory => positionHistory.Department)
            .Include(employee => employee.PositionHistories)
                .ThenInclude(positionHistory => positionHistory.Projects)
            .Where(employee => employee.PositionHistories.Any(positionHistory =>
                positionHistory.DepartmentId == departmentId &&
                positionHistory.ProjectId != null))
            .ToListAsync();
    }

    /// <summary>
    /// Crea un nuevo empleado en la base de datos y devuelve su identificador.
    /// </summary>
    /// <param name="employee">Entidad del empleado que se desea persistir.</param>
    /// <returns>Identificador generado para el empleado creado.</returns>
    public async Task<int> Create(Employee employee)
    {
        await context.Employees.AddAsync(employee);
        context.SaveChanges();

        return employee.Id;
    }

    /// <summary>
    /// Actualiza la información de un empleado existente y devuelve su identificador.
    /// </summary>
    /// <param name="employee">Entidad del empleado con los cambios aplicados.</param>
    /// <returns>Identificador del empleado actualizado.</returns>
    public async Task<int> Update(Employee employee)
    {
        context.Employees.Update(employee);
        context.SaveChanges();

        return employee.Id;
    }

    /// <summary>
    /// Elimina un empleado por su identificador.
    /// </summary>
    /// <param name="id">Identificador del empleado que se desea eliminar.</param>
    /// <returns><see langword="true"/> si el empleado existía y fue eliminado; en caso contrario, <see langword="false"/>.</returns>
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