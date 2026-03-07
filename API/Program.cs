using Application.Features.Authentication.Commands;
using Application.Interfaces;
using Application.Interfaces.Booking;
using Application.Interfaces.Customers;
using Application.Interfaces.Driver;
using Application.Interfaces.Headquarters;
using Application.Interfaces.ManagementUser;
using Application.Interfaces.ManageSales;
using Application.Interfaces.Routes;
using Application.Interfaces.vehicles;
using Infrastructure.ExternalServices;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories;
using Persistence.Repositories.Booking;
using Persistence.Repositories.Customers;
using Persistence.Repositories.Driver;
using Persistence.Repositories.Headquarters;
using Persistence.Repositories.ManageSales;
using Persistence.Repositories.Routes;
using Persistence.Repositories.vehicles;

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
builder.Services.AddScoped<IAddTravelRouteRepository, AddTravelRouteRepository>();
builder.Services.AddScoped<IGetAllTravelRoutesRepository, GetAllTravelRoutesRepository>();
builder.Services.AddScoped<IUpdateTravelRouteRepository, UpdateTravelRouteRepository>();
builder.Services.AddScoped<IDeleteTravelRouteRepository, DeleteTravelRouteRepository>();
builder.Services.AddScoped<IGetPersonnelStatisticsRepository, GetPersonnelStatisticsRepository>();
builder.Services.AddScoped<IGetStatesAccountRepository, GetStatesAccountRepository>();
builder.Services.AddScoped<IGetUserRepository, GetUserRepository>();
builder.Services.AddScoped<IEditUserRepository, EditUserRepository>();
builder.Services.AddScoped<IDeleteUserRepository, DeleteUserRepository>(); 
builder.Services.AddScoped<IGetAllPlaceofRouteRepository, GetAllPlaceofRouteRepository>();
//vehicles
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IGetAllDriverRepository, GetAllDriverRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ISearchRouteRepository, SearchRouteRepository>();
builder.Services.AddScoped<IGetDepartureTimeRepository, GetDepartureTimeRepository>();
builder.Services.AddScoped<IGetSalesRepository, GetSalesRepository>();


//BOOKING
builder.Services.AddScoped<IGetSeatRepository, GetSeatRepository>();


//TRIPS
builder.Services.AddScoped<IStartingOrderRepository, StartingOrderRepository>();
builder.Services.AddScoped<ITripsRepository, TripsRepository>();


// Lugares (Places)
builder.Services.AddScoped<IAddPlaceRepository, AddPlaceRepository>();
builder.Services.AddScoped<IGetAllPlacesRepository, GetAllPlacesRepository>();
builder.Services.AddScoped<IUpdatePlaceRepository, UpdatePlaceRepository>();
builder.Services.AddScoped<IDeletePlaceRepository, DeletePlaceRepository>();
builder.Services.AddScoped<IGenerateUniqueEmailRepository, GenerateUniqueEmailRepository>();
builder.Services.AddScoped<IHeadquarterRepository, HeadquarterRepository>();

//Routes
builder.Services.AddScoped<IRouteRepository, RouteRepository>();

//MANAGE SALES
builder.Services.AddScoped<IGetFilterRepository, GetFilterRepository>();

// 4. Conexión a SQL Server en AWS RDS
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient<IReniecService, ApisPeruService>(client =>
{
    // La URL base que te dieron en el correo
    client.BaseAddress = new Uri("https://dniruc.apisperu.com/api/v1/");

    // Opcional: Configurar tiempos de espera
    client.Timeout = TimeSpan.FromSeconds(10);
});

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

