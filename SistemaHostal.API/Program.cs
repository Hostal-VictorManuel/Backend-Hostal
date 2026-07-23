using System.Text;
using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using SistemaHostal.Application.Common;
using SistemaHostal.Application.Identidad;
using SistemaHostal.Infrastructure.Identidad;
using SistemaHostal.Infrastructure.Persistence;
using SistemaHostal.Domain.Identidad;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Infrastructure.Catalogo;
using SistemaHostal.Domain.Pagos;
using SistemaHostal.Application.Pagos;
using SistemaHostal.Infrastructure.Pagos;
using SistemaHostal.Application.Turnos;
using SistemaHostal.Infrastructure.Turnos;
using SistemaHostal.Application.Ventas;
using SistemaHostal.Infrastructure.Ventas;
using SistemaHostal.Application.Inventario;
using SistemaHostal.Infrastructure.Inventario;
using SistemaHostal.Application.Auditoria;
using SistemaHostal.Application.Notificaciones;
using SistemaHostal.Infrastructure.Auditoria;
using SistemaHostal.Application.Reportes;
using SistemaHostal.Infrastructure.Notificaciones;
using SistemaHostal.Infrastructure.Reportes;


var builder = WebApplication.CreateBuilder(args);

// --- Persistencia ---
builder.Services.AddDbContext<SistemaHostalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// --- MediatR + FluentValidation ---
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RegistrarUsuarioCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(RegistrarUsuarioCommand).Assembly);

// --- Identidad: repositorios, queries y servicios ---
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IIntentoAccesoRepository, IntentoAccesoRepository>();
builder.Services.AddScoped<IUsuarioQueries, UsuarioQueries>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaQueries, CategoriaQueries>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoQueries, ProductoQueries>();
builder.Services.AddScoped<IMetodoDePagoQueries, MetodoDePagoQueries>();
builder.Services.AddScoped<ITurnoRepository, TurnoRepository>();
builder.Services.AddScoped<ITurnoQueries, TurnoQueries>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<IVentaQueries, VentaQueries>();
builder.Services.AddScoped<INumeroVentaGenerator, NumeroVentaGenerator>();
builder.Services.AddScoped<IMovimientoInventarioRepository, MovimientoInventarioRepository>();
builder.Services.AddScoped<IMovimientoInventarioQueries, MovimientoInventarioQueries>();
builder.Services.AddScoped<IMetodoDePagoRepository, MetodoDePagoRepository>();
builder.Services.AddScoped<IRegistroBitacoraRepository, RegistroBitacoraRepository>();
builder.Services.AddScoped<IRegistroBitacoraQueries, RegistroBitacoraQueries>();
builder.Services.AddScoped<IReportesQueries, ReportesQueries>();
builder.Services.AddScoped<INotificacionRepository, NotificacionRepository>();
builder.Services.AddScoped<INotificacionQueries, NotificacionQueries>();
builder.Services.Configure<SistemaHostal.API.Seguridad.ApiKeySettings>(builder.Configuration.GetSection("ApiKeySettings"));


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// --- Autenticación JWT ---
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

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
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    })
    .AddScheme<AuthenticationSchemeOptions, SistemaHostal.API.Seguridad.ApiKeyAuthenticationHandler>(
    SistemaHostal.API.Seguridad.ApiKeyAuthenticationHandler.SchemeName, options => { });


builder.Services.AddAuthorization();

// --- Controllers + Swagger ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa: Bearer {tu token}"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// --- CORS (restringido a los orígenes conocidos del frontend) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SistemaHostalDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

    context.Database.Migrate();

    if (!context.Usuarios.Any())
    {
        var passwordHash = passwordHasher.Hash("Admin123!");
        var admin = new Usuario("Administrador General", "admin", passwordHash, RolUsuario.Administrador);
        context.Usuarios.Add(admin);
    }

    if (!context.MetodosDePago.Any())
    {
        context.MetodosDePago.AddRange(
            new MetodoDePago("Efectivo"),
            new MetodoDePago("Tarjeta"),
            new MetodoDePago("Yape"),
            new MetodoDePago("Plin"));
    }
    context.SaveChanges();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Default");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();