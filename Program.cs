using Microsoft.EntityFrameworkCore;

//* ========================================
//* CONFIGURACIÓN PRINCIPAL DE LA APLICACIÓN
//* ========================================

//* ===== PASO 1: CREACIÓN DEL BUILDER =====
// El builder configura todos los servicios y opciones antes de ejecutar la app
var builder = WebApplication.CreateBuilder(args);

//* ===== PASO 2: CONFIGURACIÓN DE SERVICIOS (DEPENDENCY INJECTION) =====
// Aquí registramos TODOS los servicios que la aplicación necesita

//? CONFIGURACIÓN DE LA BASE DE DATOS
// Obtiene la cadena de conexión desde appsettings.json
var dbConnectionString = builder.Configuration.GetConnectionString("Conexionsql");

// Registra el DbContext en el contenedor de inyección de dependencias
// AddDbContext = registra ApplicationDbContext como servicio SCOPED
// SCOPED = una nueva instancia por cada petición HTTP (se destruye al terminar la request)
// UseSqlServer = configura EF Core para usar SQL Server como base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(dbConnectionString));

//TODO: REGISTRAR EL REPOSITORY
//! Descomentar esta línea cuando implementes el repository:
// builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//? ¿Qué hace? Le dice a ASP.NET: "Cuando alguien pida ICategoryRepository, dale CategoryRepository"

//? CONFIGURACIÓN DE CONTROLADORES
// Habilita el uso de controllers (API endpoints)
builder.Services.AddControllers();

//? CONFIGURACIÓN DE SWAGGER (Documentación interactiva de la API)
// Swagger genera una UI donde puedes ver y probar todos tus endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//* ===== PASO 3: CONSTRUCCIÓN DE LA APLICACIÓN =====
// Crea la aplicación web con todos los servicios configurados
var app = builder.Build();

//* ===== PASO 4: PIPELINE DE MIDDLEWARE =====
//! IMPORTANTE: El orden de los middlewares es CRÍTICO

// SWAGGER (solo en desarrollo)
// Accede a: https://localhost:XXXX/swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();      // Endpoint JSON con la definición de la API
    app.UseSwaggerUI();    // Interfaz visual para probar la API
}

// REDIRECCIÓN HTTPS
// Redirige automáticamente HTTP → HTTPS (seguridad)
app.UseHttpsRedirection();

// AUTORIZACIÓN
// Middleware para manejar autenticación/autorización (JWT, cookies, etc.)
app.UseAuthorization();

// MAPEO DE CONTROLADORES
// Conecta las rutas HTTP con los métodos de los controladores
// Ejemplo: GET /api/categories → CategoriesController.GetCategories()
app.MapControllers();

//* ===== PASO 5: EJECUCIÓN =====
// Inicia el servidor y comienza a escuchar peticiones HTTP
app.Run();
