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

public class DepartmentEmployeesControllerTests
{
    [Fact]
    public async Task GetEmployeesWithProjectsByDepartment_WhenDepartmentIdIsInvalid_ReturnsBadRequest()
    {
        using var context = CreateContext();
        var controller = CreateController(context);

        var result = await controller.GetEmployeesWithProjectsByDepartment(0);

        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task GetEmployeesWithProjectsByDepartment_WhenEmployeesMatchFilter_ReturnsOkWithPositionHistories()
    {
        using var context = CreateContext();
        SeedData(context);
        var controller = CreateController(context);

        var result = await controller.GetEmployeesWithProjectsByDepartment(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var employeePositionHistories = Assert.IsAssignableFrom<IEnumerable<EmployeePositionHistoryDto>>(okResult.Value).ToList();

        Assert.Single(employeePositionHistories);
        Assert.Equal(1, employeePositionHistories[0].EmployeeId);
        Assert.Equal(1, employeePositionHistories[0].DepartmentId);
        Assert.Equal("Ventas", employeePositionHistories[0].DepartmentName);
        Assert.Equal(1, employeePositionHistories[0].ProjectId);
        Assert.Equal("Proyecto A", employeePositionHistories[0].ProjectName);
    }

    [Fact]
    public async Task GetEmployeesWithProjectsByDepartment_WhenNoEmployeesMatchFilter_ReturnsEmptyList()
    {
        using var context = CreateContext();
        SeedData(context);
        var controller = CreateController(context);

        var result = await controller.GetEmployeesWithProjectsByDepartment(99);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var employeePositionHistories = Assert.IsAssignableFrom<IEnumerable<EmployeePositionHistoryDto>>(okResult.Value);

        Assert.Empty(employeePositionHistories);
    }

    private static DepartmentController CreateController(MysqlDbContext context)
    {
        var repository = new EmployeeRepository(context);
        var service = new EmployeesService(repository, CreateMapper());
        return new DepartmentController(service);
    }

    private static MysqlDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MysqlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new MysqlDbContext(options);
    }

    private static void SeedData(MysqlDbContext context)
    {
        context.Employees.AddRange(
            new Employee { Id = 1, Name = "Ana", PositionId = 1, Salary = 1000m },
            new Employee { Id = 2, Name = "Luis", PositionId = 2, Salary = 2000m },
            new Employee { Id = 3, Name = "Carla", PositionId = 3, Salary = 3000m });

        context.Departments.AddRange(
            new Department { Id = 1, Name = "Ventas" },
            new Department { Id = 2, Name = "TI" });

        context.Projects.AddRange(
            new Project { Id = 1, Name = "Proyecto A" },
            new Project { Id = 2, Name = "Proyecto B" });

        context.PositionHistory.AddRange(
            new PositionHistory
            {
                Id = 1,
                EmployeeId = 1,
                PositionId = 1,
                DepartmentId = 1,
                ProjectId = 1,
                StartDate = DateTime.UtcNow.AddDays(-10),
                Description = "Historial 1",
                ReasonChange = "Ingreso"
            },
            new PositionHistory
            {
                Id = 2,
                EmployeeId = 2,
                PositionId = 2,
                DepartmentId = 1,
                ProjectId = null,
                StartDate = DateTime.UtcNow.AddDays(-8),
                Description = "Historial 2",
                ReasonChange = "Movimiento"
            },
            new PositionHistory
            {
                Id = 3,
                EmployeeId = 3,
                PositionId = 3,
                DepartmentId = 2,
                ProjectId = 2,
                StartDate = DateTime.UtcNow.AddDays(-5),
                Description = "Historial 3",
                ReasonChange = "Asignación"
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
