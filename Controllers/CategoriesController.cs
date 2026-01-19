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
        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public IActionResult GetCategory(int id)
        {
            // Verifica si la categoría existe
            if (!_categoryRepository.CategoryExists(id))
            {
                return NotFound($"Category {id} not exists"); // Retorna 404 si no existe
            }

            // Obtiene la categoría desde el repositorio
            var category = _categoryRepository.GetCategory(id);

            // Convierte la categoría a DTO usando AutoMapper
            var categoryDto = _mapper.Map<CategoryDto>(category);

            // Retorna la categoría en formato DTO
            return Ok(categoryDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //*FromBody indica que los datos vienen en el cuerpo de la solicitud HTTP
        //*Otro ejemplos: [FromQuery] (viene en la URL), [FromRoute] (viene en la ruta)
        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            //*ModelState representa el estado de validación del modelo, es decir, si los datos recibidos cumplen con las reglas definidas en el DTO
            if(createCategoryDto == null)
            {
                return BadRequest(ModelState);
            }
            if(_categoryRepository.CategoryExists(createCategoryDto.Name))
            {
                ModelState
                .AddModelError("CustomError", "Category already exists!");
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(createCategoryDto);
            
            if(!_categoryRepository.CreateCategory(category))
            {
                //*EL modelState es un diccionario que contiene el estad de validación del modelo que se recibio en la peticion
                //* almacena errores de validación y otros mensajes relacionados con el modelo
                //*valida las data annotations y otras reglas de validación definidas en el DTO
                ModelState.AddModelError("CustomError", "Something went wrong while saving the category");
                return StatusCode(500, ModelState);
            }
            //* CreatedAtRoute es un metodo de ASP.NET que se usa despues de crear un recurso exitosamente
            //* retorna un codigo de estado 201 (Created), agrega la cabecera Location con la URL del nuevo recurso
            //* y opcionalmente incluye el recurso creado en el cuerpo de la respuesta
            return CreatedAtRoute("GetCategory", new {id = category.Id}, category);
        }

        [HttpPatch("id:int", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //*FromBody indica que los datos vienen en el cuerpo de la solicitud HTTP
        //*Otro ejemplos: [FromQuery] (viene en la URL), [FromRoute] (viene en la ruta)
        public IActionResult UpdateCategory(int id, [FromBody] CreateCategoryDto updateCategoryDto)
        {
            if(!_categoryRepository.CategoryExists(id))
            {
                return NotFound($"Category {id} not exists");
            }
            //*ModelState representa el estado de validación del modelo, es decir, si los datos recibidos cumplen con las reglas definidas en el DTO
            if(updateCategoryDto == null)
            {
                return BadRequest(ModelState);
            }
            
            if(_categoryRepository.CategoryExists(updateCategoryDto.Name))
            {
                ModelState
                .AddModelError("CustomError", "Category already exists!");
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(updateCategoryDto);
            category.Id = id; // Asegura que el ID sea el correcto
            
            if(!_categoryRepository.UpdateCategory(category))
            {
               
                ModelState.AddModelError("CustomError", "Something went wrong while updating the category");
                return StatusCode(500, ModelState);
            }
           //* NoContent() = Retorna un código 204 indicando que la operación fue exitosa pero no hay contenido para retornar
            return NoContent();
        }

        [HttpDelete("id:int", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //*FromBody indica que los datos vienen en el cuerpo de la solicitud HTTP
        //*Otro ejemplos: [FromQuery] (viene en la URL), [FromRoute] (viene en la ruta)
        public IActionResult DeleteCategory(int id)
        {
            if(!_categoryRepository.CategoryExists(id))
            {
                return NotFound($"Category {id} not exists");
            }

            var category = _categoryRepository.GetCategory(id);

             if(category == null)
            {
                return NotFound($"Category {id} not exists");
            }
            
            if(!_categoryRepository.DeleteCategory(category))
            {
               
                ModelState.AddModelError("CustomError", "Something went wrong while deleting the category");
                return StatusCode(500, ModelState);
            }
           //* NoContent() = Retorna un código 204 indicando que la operación fue exitosa pero no hay contenido para retornar
            return NoContent();
        }

    }
}
