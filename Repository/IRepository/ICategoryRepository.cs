using System;
namespace webApi.Repository.IRespository;

//* ========================================
//* INTERFAZ - CONTRATO DEL REPOSITORY
//* ========================================
//? ¿Qué es esto? Define el CONTRATO que debe cumplir cualquier Repository de Categorías
//? ¿Para qué sirve? Permite desacoplar - los controladores usan ESTA interfaz, no la implementación
//? ¿Ventaja? Puedes cambiar la implementación sin tocar los controladores (Open/Closed Principle)

public interface ICategoryRepository
{
    //* ===== MÉTODOS DE CONSULTA (READ) =====
    
    // Obtiene TODAS las categorías de la base de datos
    // Retorna: Colección de todas las categorías
    ICollection<Category> GetCategories();
    
    // Obtiene UNA categoría específica por su ID
    // Parámetro: id - ID de la categoría a buscar
    // Retorna: La categoría encontrada o null si no existe
    Category GetCategory(int id);
    
    // Verifica si una categoría EXISTE en la BD por ID numérico
    // Parámetro: id - ID de la categoría a verificar
    // Retorna: true si existe, false si no
    bool CategoryExists(int id);
    
    // Verifica si una categoría EXISTE en la BD por nombre
    // Parámetro: name - Nombre de la categoría a verificar
    // Retorna: true si existe, false si no
    bool CategoryExists(string id);

    //* ===== MÉTODOS DE MODIFICACIÓN (CREATE, UPDATE, DELETE) =====
    
    // Crea una NUEVA categoría en la base de datos
    // Parámetro: category - Objeto Category con los datos a crear
    // Retorna: true si se creó exitosamente, false si falló
    bool CreateCategory(Category category);
    
    // Actualiza una categoría EXISTENTE en la base de datos
    // Parámetro: category - Objeto Category con los datos actualizados
    // Retorna: true si se actualizó exitosamente, false si falló
    bool UpdateCategory(Category category);
    
    // Elimina una categoría de la base de datos
    // Parámetro: category - Objeto Category a eliminar
    // Retorna: true si se eliminó exitosamente, false si falló
    bool DeleteCategory(Category category);
    
    // Guarda TODOS los cambios pendientes en la base de datos
    //! IMPORTANTE: Este método ejecuta SaveChanges() de Entity Framework
    // Retorna: true si se guardaron los cambios, false si falló
    bool Save();
}