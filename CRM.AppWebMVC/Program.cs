var builder = WebApplication.CreateBuilder(args); //crea un constructor de aplicaciones web

// Agrega servicios al contenedor de dependecias.
builder.Services.AddControllersWithViews(); //Agregar servicios para contralodares y vistas

//configura y agrega un cliente HTTP con nombe "CRMAPI"
builder.Services.AddHttpClient("CRMAPI", c =>
{
    //Configurar la direccion base del cliente HTTP desde la configuracion
    c.BaseAddress = new Uri(builder.Configuration["UrlsAPI:CRM"]);
    //Puedes configurar otras opciones del HTTPClient aqaui segun sea necesario
});

var app = builder.Build(); //Crear una instancia de la aplicacion web

// Configura el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    //Maneja excepciones en caso de errores y redirige a la accion "Eror" en el controlador "Home"
    app.UseExceptionHandler("/Home/Error");
    // El valor HSTS predeterminado es de 30 dias. Puedes cambiarlo para escenarios de produccion.
    app.UseHsts();
}

app.UseHttpsRedirection(); //Rdirige las solicitudes HTTP a HTTPS
app.UseStaticFiles(); //Habilita el uso de archivos estaticos como CSS, Javascript, imagenes, etc.

app.UseRouting(); //Configura el enrutamiento de solicitudes

app.UseAuthorization(); //Habilita la autorizacion para proteger rutas y acciones de controladores

//Mapea la ruta predeterminada de controlador y accion
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); //Inicia la aplicacion
