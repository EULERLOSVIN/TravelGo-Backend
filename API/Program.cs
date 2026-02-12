using Application.Features.Authentication.Commands;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de CORS (Fundamental para Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("TravelGoPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // URL de tu frontend en Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 2. Registro de MediatR (Capa de Application)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserHandler).Assembly));

// 3. Inyección de Dependencias (Repositorios)
builder.Services.AddScoped<IRegisterUserRepository, RegisterUserRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IGetPersonnelRepository, GetPersonnelRepository>();
builder.Services.AddScoped<IGetAllRolesRepository, GetAllRolesRepositoy>();
builder.Services.AddScoped<IGetTypesDocumentsRepository, GetTypesDocumentsRepository>();
builder.Services.AddScoped<IGetPersonnelStatisticsRepository, GetPersonnelStatisticsRepository>();
builder.Services.AddScoped<IGetStatesAccountRepository, GetStatesAccountRepository>();
builder.Services.AddScoped<IGetUserRepository, GetUserRepository>();
builder.Services.AddScoped<IEditUserRepository, EditUserRepository>();
builder.Services.AddScoped<GenerateUniqueEmailRepository>();
builder.Services.AddScoped<IDeleteUserRepository, DeleteUserRepository>(); //

// 4. Conexión a SQL Server en AWS RDS
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Configuración del Pipeline de HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 6. Habilitar CORS (Debe ir antes de la Autorización)
app.UseCors("TravelGoPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();