using Microsoft.AspNetCore.Mvc;
using TiendaApi.Interfaces;
using TiendaApi.Models;

namespace TiendaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepository _repository;
        public ProductoController(IProductoRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IActionResult TraerProductos()
        {
            var res = _repository.TraerProductos();
            return Ok(res);
        }
        [HttpGet("{codigo}")]
        public IActionResult TraerProducto(uint codigo)
        {
            var res = _repository.TraerProducto(codigo);
            if (res is null) return NotFound("El producto no existe.");
            return Ok(res);
        }
        [HttpPut]
        public IActionResult ActualizarProducto([FromBody] Producto producto)
        {
            if (producto is null) return BadRequest("El producto es Nulo");
            var res = _repository.ActualizarProducto(producto);
            if (!res) return UnprocessableEntity("El producto no se pudo actualizar. Verifica que no exista ya.");
            return Ok($"El producto con ID:'{producto.Codigo}' se actualizo corractamente.");
        }
        [HttpDelete("{codigo}")]
        public IActionResult BorrarProducto(uint codigo)
        {
            var res = _repository.EliminarProducto(codigo);
            if (!res) return UnprocessableEntity("El producto no se pudo borrar. Es posible que no exista.");
            return Ok("El producto ha sido borrado.");
        }
        [HttpPost]
        public IActionResult AgregarProducto([FromBody] Producto producto)
        {
            if (producto is null) return BadRequest("El producto es Nulo");
            var res = _repository.AgregarProducto(producto);
            if (res == 0) return BadRequest("No se pudo agregar el producto.");
            return Ok(new { Message = "Se agrego correctamente", NewId = res });
        }
    }
}
