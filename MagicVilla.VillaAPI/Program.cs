//using Serilog;

using MagicVilla.VillaAPI;
using MagicVilla.VillaAPI.Data;
using MagicVilla.VillaAPI.Logging;
using MagicVilla.VillaAPI.Repository;
using MagicVilla.VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionBD"));
});
builder.Services.AddResponseCaching();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;

});

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false

    };


});

#region LoggerEjemplo
// Este fragmento de codigo sirve para crear un archivo .txt con los mensajes de Logger de la Api.
// En caso de no utilizarlo simplemente comentar y desintalar los paquetes: serilog.Sinsk.file and serilog.AspNetCore
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo
//    .File("log/VillaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

//builder.Host.UseSerilog();
#endregion

builder.Services.AddControllers( options => { options.ReturnHttpNotAcceptable = true;}) //ReturnHttpNotAcceptable definir que nuestra Api solo acepta datos en tipo json
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters(); //Acepte data en tipo xml y regrese(output) en tipo xml
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( opciones =>
{
    opciones.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = 
        "JWT Authorization header usa el Bearer esquema. \r\n\r\n " +
        "Enter 'Bearer' [espacio] y pegar el [token] obtenido del metodo Login. \r\n\r\n" +
        "Ejemplo: \"Bearer 12hdhsgusjs...ss\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    opciones.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    opciones.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Magic Villa 1.0",
        Description = "API para administrar Villa",
        TermsOfService = new Uri("https://example.com/term"),
        Contact = new OpenApiContact
        {
            Name = "Dotnetmastery",
            Url = new Uri("https://dotnetmastery.com")
        },
        License = new OpenApiLicense
        {
            Name = "Ejemplo Licencia",
            Url = new Uri("https://example.com/license")
        }

    });

    opciones.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Magic Villa 2.0",
        Description = "API para administrar Villa 2.0",
        TermsOfService = new Uri("https://example.com/term"),
        Contact = new OpenApiContact
        {
            Name = "Dotnetmastery",
            Url = new Uri("https://dotnetmastery.com")
        },
        License = new OpenApiLicense
        {
            Name = "Ejemplo Licencia",
            Url = new Uri("https://example.com/license")
        }

    });
}
    
    
);
builder.Services.AddSingleton<ILogging, Logging>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic_VillaV1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
