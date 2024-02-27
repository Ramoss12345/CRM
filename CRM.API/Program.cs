//Importar los espacios de nombres necesarios para el proyecto
using CRM.API.Endpoints;
using CRM.API.Models.DAL;
using Microsoft.EntityFrameworkCore;

//Crear un nuevo constructor de la aplicacion web
var builder = WebApplication.CreateBuilder(args);

//Agregar servicios para habilitar la generacion de documento api
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configurar y agregar un contexto de base de datos para entity
builder.Services.AddDbContext<CMRContext>(Options =>
Options.UseSqlServer(builder.Configuration.GetConnectionString("Conn"))
);

//Configura y agrega un contexto de base de datos para Entity Framework Core
builder.Services.AddScoped<CustomerDAL>();

//construye la aplicacion web
var app = builder.Build();

//Agrega los puntos finales relacionados con los clientes a la aplicacion
app.AddCustomerEndpoints();

// Verifica si la aplicacion se esta ejecutando en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    //Habilitar el uso de Swagger para la documentacion de la api
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Agregar middleare para redirigir la solucitudes HTTP a HTTPS
app.UseHttpsRedirection();

//Ejecutar la aplicacion
app.Run();
