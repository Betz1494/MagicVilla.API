//using Serilog;

using MagicVilla.VillaAPI.Data;
using MagicVilla.VillaAPI.Logging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionBD"));
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
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogging, Logging>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
