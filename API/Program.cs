using Application.Features.Authentication.Commands;
using Application.Interfaces;
using Application.Interfaces.Settings;
 
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

using Persistence.Repositories.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Persistence.Repositories.ManageSales;
using Persistence.Repositories.Routes;
using Persistence.Repositories.vehicles;
using Application.Interfaces.QueueVehicles;
using Persistence.Repositories.QueueVehicles;
using Application.Interfaces.DepartureTimes;
using Persistence.Repositories.DepartureTimes;
using Application.Interfaces.SecurityAlerts;
using Persistence.Repositories.SecurityAlerts;
using Application.Interfaces.Dashboard;
using Persistence.Repositories.Dashboard;

 

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

// Settings
builder.Services.AddScoped<ISettingCompanyRepository, SettingCompanyRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

//MANAGE SALES
builder.Services.AddScoped<IGetFilterRepository, GetFilterRepository>();

// QUEUE VEHICLES
builder.Services.AddScoped<IAddQueueVehicleRepository, AddQueueVehicleRepository>();
builder.Services.AddScoped<IDeleteQueueVehicleRepository, DeleteQueueVehicleRepository>();
builder.Services.AddScoped<IGetActiveQueueRepository, GetActiveQueueRepository>();
builder.Services.AddScoped<IGetDriverQueueInfoRepository, GetDriverQueueInfoRepository>();
builder.Services.AddScoped<IUpdateQueueVehicleRouteRepository, UpdateQueueVehicleRouteRepository>();
builder.Services.AddScoped<IRegisterArrivalRepository, RegisterArrivalRepository>();
builder.Services.AddScoped<IGetRoutesByHeadquarterRepository, GetRoutesByHeadquarterRepository>();
builder.Services.AddScoped<IDispatchVehicleRepository, DispatchVehicleRepository>();

// DEPARTURE TIMES
builder.Services.AddScoped<IAddDepartureTimeRepository, AddDepartureTimeRepository>();
builder.Services.AddScoped<IDeleteDepartureTimeRepository, DeleteDepartureTimeRepository>();
builder.Services.AddScoped<IGetDepartureTimesByRouteRepository, GetDepartureTimesByRouteRepository>();

// SECURITY ALERTS
builder.Services.AddScoped<IGetSecurityAlertsRepository, GetSecurityAlertsRepository>();

// DASHBOARD
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Seed database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (!context.Companies.Any())
        {
            context.Companies.Add(new Persistence.Company
            {
                BusinessName = "TRAVEL GO",
                Ruc = "20000000001",
                Phone = "000-000000",
                Email = "admin@travelgo.com",
                FiscalAddress = "Fiscal Address 123",
                RegistrationDate = DateTime.Now
            });
        }

        if (!context.StateHeadquarters.Any())
        {
            context.StateHeadquarters.AddRange(
                new Persistence.StateHeadquarter { Name = "Activa" },
                new Persistence.StateHeadquarter { Name = "Inactiva" }
            );
        }
        
        context.SaveChanges();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// 5. Configuración del Pipeline de HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 6. Habilitar CORS (Debe ir antes de la Autorización)
app.UseCors("TravelGoPolicy");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

