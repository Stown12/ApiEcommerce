using System;
using AutoMapper;
using webApi.Models.Dtos;

namespace webApi.Mapping;

//* ========================================
//* PERFIL DE AUTOMAPPER - CATEGORY
//* ========================================
//? ¿Qué es esto? Define las reglas de mapeo entre entidades y DTOs
//? ¿Para qué sirve? Simplifica la conversión entre objetos (Category ↔ DTOs)
//? ¿Cómo funciona? Usa AutoMapper para automatizar el mapeo

public class CategoryProfile : Profile
{
    //* ===== CONSTRUCTOR =====
    //? Aquí defines las reglas de mapeo entre tipos
    public CategoryProfile()
    {
        //? Mapeo entre Category y CategoryDto (y viceversa)
        // ReverseMap() = Permite el mapeo en ambas direcciones
        CreateMap<Category, CategoryDto>().ReverseMap();

        //? Mapeo entre Category y CreateCategoryDto (y viceversa)
        // Esto es útil para manejar datos de entrada al crear una categoría
        CreateMap<Category, CreateCategoryDto>().ReverseMap();
    }
}
