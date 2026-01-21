using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webApi.Models;
using webApi.Models.Dtos;
using webApi.Repository.IRepository;
using webApi.Repository.IRespository;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        //? GET: api/products
        //? Retorna todos los productos en formato DTO
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProducts()
        {
            // Obtiene todas las categorías desde el repositorio
            var products = _productRepository.GetProducts();

            // Convierte las categorías a DTO usando AutoMapper
            var productsDto = _mapper.Map<List<ProductDto>>(products);
            

            // Retorna la lista de categorías en formato DTO
            return Ok(productsDto);
        }

        [HttpGet("{productId:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public IActionResult GetProduct(int productId)
        {
            // Verifica si la categoría existe
            if (!_productRepository.ProductExists(productId))
            {
                return NotFound($"Product {productId} not exists"); // Retorna 404 si no existe
            }

            // Obtiene la categoría desde el repositorio
            var product = _productRepository.GetProduct(productId);
            // Convierte la categoría a DTO usando AutoMapper
            var productDto = _mapper.Map<ProductDto>(product);

            // Retorna la categoría en formato DTO
            return Ok(productDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //*FromBody indica que los datos vienen en el cuerpo de la solicitud HTTP
        //*Otro ejemplos: [FromQuery] (viene en la URL), [FromRoute] (viene en la ruta)
        public IActionResult CreateProduct([FromBody] CreateProductoDto createProductoDto)
        {
            //*ModelState representa el estado de validación del modelo, es decir, si los datos recibidos cumplen con las reglas definidas en el DTO
            if(createProductoDto == null)
            {
                return BadRequest(ModelState);
            }
            //*Verificar que el product no exista
            if(_productRepository.ProductExists(createProductoDto.Name))
            {
                ModelState
                .AddModelError("CustomError", "Product already exists!");
                return BadRequest(ModelState);
            }
            //*Verificar que la categoria exista
            if(!_categoryRepository.CategoryExists(createProductoDto.CategoryId))
            {
                ModelState
                .AddModelError("CustomError", "Category does not exist!");
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(createProductoDto);
            
            if(!_productRepository.CreateProduct(product))
            {
                //*EL modelState es un diccionario que contiene el estad de validación del modelo que se recibio en la peticion
                //* almacena errores de validación y otros mensajes relacionados con el modelo
                //*valida las data annotations y otras reglas de validación definidas en el DTO
                ModelState.AddModelError("CustomError", "Something went wrong while saving the Product");
                return StatusCode(500, ModelState);
            }
            //* CreatedAtRoute es un metodo de ASP.NET que se usa despues de crear un recurso exitosamente
            //* retorna un codigo de estado 201 (Created), agrega la cabecera Location con la URL del nuevo recurso
            //* y opcionalmente incluye el recurso creado en el cuerpo de la respuesta
            var createdProduct = _productRepository.GetProduct(product.ProductId);
            var productDto = _mapper.Map<ProductDto>(createdProduct);
            return CreatedAtRoute("GetProduct", new {productId = product.ProductId}, productDto);
        }
        
        [HttpGet("searchProductByCategory/{categoryId:int}", Name = "GetProductByCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public IActionResult GetProductByCategory(int categoryId)
        {
            var product = _productRepository.GetProductForCategory(categoryId);
            if (product.Count == 0)
            {
                return NotFound($"The products with category id: {categoryId} does not exists");
            }

            var productDto = _mapper.Map<List<ProductDto > >(product); 
            return Ok(productDto);

        }
        
        [HttpGet("searchProductByDescription/{searchTerm}", Name = "SearchProducts")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public IActionResult SearchProduct(string searchTerm)
        {
            var product = _productRepository.SearchProducts(searchTerm);
            if (product.Count == 0)
            {
                return NotFound($"The products with category id: {searchTerm} does not exists");
            }

            var productDto = _mapper.Map<List<ProductDto > >(product); 
            return Ok(productDto);

        }
    }
}
