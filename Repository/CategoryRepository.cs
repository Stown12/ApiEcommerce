using System;
using System.Data.Common;
using webApi.Repository.IRespository;

namespace webApi.Repository;

//* ========================================
//* REPOSITORY DE CATEGORÍAS
//* ========================================
//? ¿Qué es esto? Implementación concreta del patrón Repository
//? ¿Para qué sirve? Contiene la lógica REAL de acceso a datos usando Entity Framework
//? ¿Por qué? Desacopla la lógica de negocio del acceso a datos

public class CategoryRepository : ICategoryRepository
{
    //* ===== DEPENDENCIAS =====
    
    // DbContext de Entity Framework - Puente con la base de datos
    // readonly = solo se asigna en el constructor (inmutable)
    // _db = convención para campos privados
    private readonly ApplicationDbContext _db;

    //* ===== CONSTRUCTOR =====
    //? Recibe el DbContext por INYECCIÓN DE DEPENDENCIAS
    // ASP.NET Core automáticamente crea y pasa la instancia cuando alguien solicita ICategoryRepository
    public CategoryRepository(ApplicationDbContext db)
    {
        _db = db; // Guardamos la referencia para usar en todos los métodos
    }
    
    //* ===== MÉTODOS DE CONSULTA (READ) =====
    
    // Verifica si existe una categoría con el ID proporcionado
    public bool CategoryExists(int id)
    {
        // Any() = retorna true si encuentra al menos 1 elemento que cumpla la condición
        return _db.Categories.Any(c => c.Id == id);
    }

    // Verifica si existe una categoría por nombre (case-insensitive)
    public bool CategoryExists(string name)
    {
        // ToLower() = convierte a minúsculas para comparación sin importar mayúsculas
        // Trim() = elimina espacios al inicio y final
        return _db.Categories.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    // Obtiene TODAS las categorías de la base de datos
    public ICollection<Category> GetCategories()
    {
        //TODO: Implementar usando _db.Categories.OrderBy(c => c.Name).ToList()
        // OrderBy() = ordena alfabéticamente por nombre
        // ToList() = ejecuta la consulta SQL y convierte el resultado a lista
        return _db.Categories.OrderBy(c => c.Name).ToList();
    }

    // Busca y retorna UNA categoría específica por su ID
    public Category GetCategory(int id)
    {
        //TODO: Implementar usando _db.Categories.FirstOrDefault(c => c.Id == id)
        // FirstOrDefault() = retorna el primer elemento que cumple la condición, o null si no encuentra nada
        return _db.Categories.FirstOrDefault(c => c.Id == id) ?? throw new InvalidOperationException($"Category {id} not exists");
    }

    //* ===== MÉTODOS DE MODIFICACIÓN (CREATE, UPDATE, DELETE) =====
    
    // Crea una NUEVA categoría en la base de datos
    public bool CreateCategory(Category category)
    {
        //TODO: Implementar usando _db.Categories.Add(category)
        //! IMPORTANTE: Add() NO guarda inmediatamente, solo marca para inserción
        //! Debes llamar Save() después para ejecutar el INSERT en la BD
        category.CreationDate = DateTime.Now;
        _db.Categories.Add(category);
        return Save();
    }

    // Actualiza una categoría EXISTENTE
    public bool UpdateCategory(Category category)
    {
        //TODO: Implementar usando _db.Categories.Update(category)
        //! IMPORTANTE: Update() NO guarda inmediatamente, solo marca para actualización
        //! Debes llamar Save() después para ejecutar el UPDATE en la BD
        category.CreationDate = DateTime.Now;
        _db.Categories.Update(category);
        return Save();
    }

    // Elimina una categoría de la base de datos
    public bool DeleteCategory(Category category)
    {
        //TODO: Implementar usando _db.Categories.Remove(category)
        //! IMPORTANTE: Remove() NO elimina inmediatamente, solo marca para eliminación
        //! Debes llamar Save() después para ejecutar el DELETE en la BD
        _db.Categories.Remove(category);
        return Save();
    }

    // Guarda TODOS los cambios pendientes en la base de datos
    public bool Save()
    {
        //TODO: Implementar usando: return _db.SaveChanges() >= 0;
        // SaveChanges() = genera y ejecuta los comandos SQL necesarios (INSERT, UPDATE, DELETE)
        // Retorna el número de registros afectados
        // >= 0 significa éxito (incluso si no hubo cambios)
        return _db.SaveChanges() >= 0;
    }
}
