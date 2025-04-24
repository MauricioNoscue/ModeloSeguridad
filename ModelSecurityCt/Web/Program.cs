using System.Text;
using Business;
using Business.Interfaces;
using Business.Services;
using Data.Interfaces;
using Data.Repositories;
using Entity.context;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Utilities.Mapping;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var dbProvider = configuration["DatabaseProvider"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<PersonService>();

builder.Services.AddScoped<IFormRepository,FormRepository>();
builder.Services.AddScoped<FormService>();

builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<ModuleService>();

builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<PermissionService>();

builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<RolService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();


builder.Services.AddScoped<IFormModuleRepository, FormModuleRepository>();
builder.Services.AddScoped<FormModuleService>();

builder.Services.AddScoped<IRolUserRepository,RolUserRepository>();
builder.Services.AddScoped<RolUserService>();

builder.Services.AddScoped<IRolFormPermissionRepository, RolFormPermissionRepository>();
builder.Services.AddScoped<RolFormPermissionService>();
builder.Services.AddScoped<JWTService>();

builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<RolUserRepository>();
// posible causa de error
builder.Services.AddHttpContextAccessor();



builder.Services.AddMapster(); 
MapsterConfig.RegisterMappings(); 

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    switch (dbProvider)
    {
        case "SqlServer":
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            break;
        case "MySql":
            options.UseMySql(configuration.GetConnectionString("MySqlConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("MySqlConnection")));
            break;
        case "Postgres":
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
            break;
        default:
            throw new Exception("Proveedor de base de datos no soportado");
    }
});

var OrigenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")!.Split(",");

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(politica =>
    {
        politica.WithOrigins(OrigenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
        )
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
