using Do.Commission.Application;
using Do.Commission.Application.Dtos;
using Do.Commission.Application.Mapping;
using Do.Commission.Infrastructure;
using Do.Commission.Infrastructure.Model;
using Do.Commission.Infrastructure.Repositories;
using Do.Commission_.Controllers;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Do.Commissions.Test.Controllers;

public class EmployeesControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithEmployees()
    {
        using var context = CreateContext(
            new Employee { Id = 1, Name = "Ana", PositionId = 1, Salary = 1000m },
            new Employee { Id = 2, Name = "Luis", PositionId = 2, Salary = 2000m });
        SeedHistoryData(context);

        var controller = CreateController(context);

        var result = await controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var employees = Assert.IsAssignableFrom<IEnumerable<EmployeeDto>>(okResult.Value).ToList();

        Assert.Equal(2, employees.Count);
        Assert.Equal("Ana", employees[0].Name);
        Assert.Equal("Luis", employees[1].Name);
        Assert.NotEmpty(employees[0].PositionHistories);
        Assert.Contains(employees[0].PositionHistories, history => history is not null && history.DepartmentName == "Ventas" && history.ProjectName == "Proyecto A");
    }

    [Fact]
    public async Task GetById_WhenEmployeeExists_ReturnsOk()
    {
        using var context = CreateContext(new Employee { Id = 7, Name = "Ana", PositionId = 1, Salary = 1000m });
        SeedHistoryData(context, 7);
        var controller = CreateController(context);

        var result = await controller.GetById(7);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var employee = Assert.IsType<EmployeeDto>(okResult.Value);

        Assert.Equal(7, employee.Id);
        Assert.Equal("Ana", employee.Name);
        Assert.NotEmpty(employee.PositionHistories);
        Assert.Contains(employee.PositionHistories, history => history is not null && history.DepartmentId == 1 && history.ProjectId == 1);
    }

    [Fact]
    public async Task GetById_WhenEmployeeDoesNotExist_ReturnsNotFound()
    {
        using var context = CreateContext();
        var controller = CreateController(context);

        var result = await controller.GetById(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateAsync_WhenPayloadIsNull_ReturnsBadRequest()
    {
        using var context = CreateContext();
        var controller = CreateController(context);

        var result = await controller.CreateAsync(null!);

        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task CreateAsync_WhenPayloadIsValid_ReturnsCreatedAtAction()
    {
        using var context = CreateContext();
        var controller = CreateController(context);
        var employee = new EmployeeDtoIn
        {
            Name = "Mario",
            PositionId = 3,
            Salary = 3500m
        };

        var result = await controller.CreateAsync(employee);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

        Assert.Equal(nameof(EmployeesController.GetById), createdResult.ActionName);
        Assert.Same(employee, createdResult.Value);
        Assert.True(createdResult.RouteValues is not null);
        Assert.True(createdResult.RouteValues.TryGetValue("id", out var id));
        Assert.IsType<int>(id);

        var storedEmployee = await context.Employees.SingleAsync();
        Assert.Equal("Mario", storedEmployee.Name);
        Assert.Equal(3, storedEmployee.PositionId);
        Assert.Equal(3500m, storedEmployee.Salary);
    }

    [Fact]
    public async Task Update_WhenPayloadIsNull_ReturnsBadRequest()
    {
        using var context = CreateContext(new Employee { Id = 4, Name = "Ana", PositionId = 1, Salary = 1000m });
        var controller = CreateController(context);

        var result = await controller.Update(4, null!);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Update_WhenEmployeeDoesNotExist_ReturnsNotFound()
    {
        using var context = CreateContext();
        var controller = CreateController(context);
        var updated = new EmployeeDtoIn
        {
            Name = "Actualizado",
            PositionId = 2,
            Salary = 1800m
        };

        var result = await controller.Update(40, updated);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_WhenEmployeeExists_ReturnsOk()
    {
        using var context = CreateContext(new Employee { Id = 5, Name = "Ana", PositionId = 1, Salary = 1000m });
        var controller = CreateController(context);
        var updated = new EmployeeDtoIn
        {
            Name = "Carla",
            PositionId = 2,
            Salary = 2200m
        };

        var result = await controller.Update(5, updated);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(5, okResult.Value);

        var storedEmployee = await context.Employees.SingleAsync();
        Assert.Equal("Carla", storedEmployee.Name);
        Assert.Equal(2, storedEmployee.PositionId);
        Assert.Equal(2200m, storedEmployee.Salary);
    }

    [Fact]
    public async Task Delete_WhenEmployeeDoesNotExist_ReturnsNotFound()
    {
        using var context = CreateContext();
        var controller = CreateController(context);

        var result = await controller.Delete(77);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Delete_WhenEmployeeExists_ReturnsOk()
    {
        using var context = CreateContext(new Employee { Id = 9, Name = "Ana", PositionId = 1, Salary = 1000m });
        var controller = CreateController(context);

        var result = await controller.Delete(9);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.True(Assert.IsType<bool>(okResult.Value));
        Assert.Empty(context.Employees);
    }

    private static EmployeesController CreateController(MysqlDbContext context)
    {
        var repository = new EmployeeRepository(context);
        var service = new EmployeesService(repository, CreateMapper());

        return new EmployeesController(service);
    }

    private static MysqlDbContext CreateContext(params Employee[] employees)
    {
        var options = new DbContextOptionsBuilder<MysqlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new MysqlDbContext(options);

        if (employees.Length > 0)
        {
            context.Employees.AddRange(employees);
            context.SaveChanges();
        }

        return context;
    }

    private static void SeedHistoryData(MysqlDbContext context, int employeeId = 1)
    {
        context.Departments.Add(new Department { Id = 1, Name = "Ventas" });
        context.Projects.Add(new Project { Id = 1, Name = "Proyecto A" });
        context.PositionHistory.Add(new PositionHistory
        {
            Id = 1,
            EmployeeId = employeeId,
            PositionId = 1,
            DepartmentId = 1,
            ProjectId = 1,
            StartDate = DateTime.UtcNow.AddDays(-10),
            Description = "Historial 1",
            ReasonChange = "Ingreso"
        });
        context.SaveChanges();
    }

    private static IMapper CreateMapper()
    {
        var config = new TypeAdapterConfig();
        config.Apply(new ConfigMapping());
        return new Mapper(config);
    }
}
