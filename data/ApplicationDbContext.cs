using Microsoft.EntityFrameworkCore;

//* ========================================
//* DB CONTEXT - PUENTE CON LA BASE DE DATOS
//* ========================================
//? ¿Qué es esto? El contexto de Entity Framework que representa una SESIÓN con la BD
//? ¿Para qué sirve? Es el puente entre tu aplicación C# y SQL Server
//? Hereda de DbContext que proporciona toda la funcionalidad de EF Core

public class ApplicationDbContext : DbContext
{
    //* ===== CONSTRUCTOR =====
    // Recibe las opciones de configuración: cadena de conexión, proveedor (SQL Server), etc.
    // base(options) = pasa las opciones al constructor de la clase padre (DbContext)
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        // No necesitamos código aquí - DbContext maneja todo internamente
    }

    //* ===== TABLAS (DbSet) =====
    
    // DbSet<Category> = Representa la TABLA "Categories" en SQL Server
    // Permite hacer consultas LINQ sobre esta tabla
    // Ejemplos: Categories.Where(...), Categories.Add(...), Categories.Remove(...)
    public DbSet<Category> Categories { get; set; }
    
    //? ¿Quieres agregar más tablas? Crea más DbSet aquí
    // Ejemplo: public DbSet<Product> Products { get; set; }
}