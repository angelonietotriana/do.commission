using Do.Commission.Application;
using Do.Commission.Domain.Commission;
using Do.Commission.Infrastructure;
using Do.Commission.Infrastructure.Repositories;
using Do.Commission_.Auth;
using Microsoft.EntityFrameworkCore;
using Mapster;
using MapsterMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuración conexión a Mysql

builder.Services.AddDbContext<MysqlDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<EmployeesService>();
builder.Services.AddScoped<ICommissionsService, CommissionsService>();
builder.Services.AddScoped<ICommissionCalculator, CommissionCalculator>();
builder.Services.AddSingleton<CommissionCalculator, CommissionCalculator>();
builder.Services.AddSingleton<InMemoryUserStore>();
builder.Services.AddSingleton<JwtTokenService>();

var config = new Mapster.TypeAdapterConfig();
config.Apply(new Do.Commission.Application.Mapping.ConfigMapping());

builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<Do.Commission_.Middleware.Middleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();


