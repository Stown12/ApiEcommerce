using System.ComponentModel.DataAnnotations;

//* ========================================
//* MODELO DE DATOS - CATEGORÍA
//* ========================================
//? ¿Qué es esto? Representa una CATEGORÍA en la base de datos
//? ¿Cómo funciona? EF Core convierte esta clase en una TABLA en SQL Server
//? Cada propiedad → Columna de la tabla
//? Data Annotations ([Key], [Required]) → Reglas de la BD

public class Category
{
    //* ===== PROPIEDADES =====
    
    // ID único de la categoría (Clave Primaria)
    // [Key] = PRIMARY KEY en SQL Server
    // Por convención, EF también la hace IDENTITY (auto-incremental)
    [Key]
    public int Id { get; set; }
    
    // Nombre de la categoría
    // [Required] = NOT NULL en SQL Server (campo obligatorio)
    // string.Empty = valor por defecto para evitar null
    [Required]
    public string Name { get; set; } = string.Empty;
    
    // Fecha de creación de la categoría
    // [Required] = NOT NULL en SQL Server
    // DateTime = Se convierte en datetime2 en SQL Server
    [Required]
    public DateTime CreationDate { get; set; }
    
    //? ¿Necesitas más propiedades?
    // Ejemplo con campo opcional (nullable):
    // public string? Description { get; set; }
    // El '?' indica que puede ser null
}