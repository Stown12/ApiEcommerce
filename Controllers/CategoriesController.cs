using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webApi.Models.Dtos;
using webApi.Repository.IRespository;

namespace webApi.Controllers
{
    //* ========================================
    //* CONTROLADOR - CATEGORIES
    //* ========================================
    //? ¿Qué es esto? Controlador para manejar las operaciones relacionadas con categorías
    //? ¿Cómo funciona? Define endpoints HTTP para interactuar con la API
    //? ¿Qué usa? ICategoryRepository para acceso a datos y AutoMapper para conversión de objetos

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        //* ===== DEPENDENCIAS =====
        
        //? Repositorio para manejar la lógica de acceso a datos
        private readonly ICategoryRepository _categoryRepository;
        
        //? AutoMapper para convertir entre entidades y DTOs
        private readonly IMapper _mapper;

        //* ===== CONSTRUCTOR =====
        //? Recibe las dependencias por inyección de dependencias
        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        //* ===== ENDPOINTS =====
        
        //? GET: api/categories
        //? Retorna todas las categorías en formato DTO
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Respuesta si el acceso está prohibido
        [ProducesResponseType(StatusCodes.Status200OK)]       // Respuesta exitosa
        public IActionResult GetCategories()
        {
            // Obtiene todas las categorías desde el repositorio
            var categories = _categoryRepository.GetCategories();

            // Convierte las categorías a DTO usando AutoMapper
            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(_mapper.Map<CategoryDto>(category));
            }

            // Retorna la lista de categorías en formato DTO
            return Ok(categoriesDto);
        }
        

        //* se define el endpoint para obtener una categoría por su ID
        //? GET: api/categories/{id}
        [HttpGet("{id: int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public IActionResult GetCategory(int id)
        {
            // Verifica si la categoría existe
            if (!_categoryRepository.CategoryExists(id))
            {
                return NotFound(); // Retorna 404 si no existe
            }

            // Obtiene la categoría desde el repositorio
            var category = _categoryRepository.GetCategory(id);

            // Convierte la categoría a DTO usando AutoMapper
            var categoryDto = _mapper.Map<CategoryDto>(category);

            // Retorna la categoría en formato DTO
            return Ok(categoryDto);
        }

    }
}
