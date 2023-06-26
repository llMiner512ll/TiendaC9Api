using Microsoft.AspNetCore.Mvc;
using TiendaApi.Interfaces;
using TiendaApi.Models;

namespace TiendaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FabricanteController : ControllerBase
    {
        private readonly IFabricanteRepository _repository;
        public FabricanteController(IFabricanteRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IActionResult TraerFabricantes()
        {
            var res = _repository.TraerFabricantes();
            return Ok(res);
        }
        [HttpGet("{codigo}")]
        public IActionResult TraerFabricante(uint codigo)
        {
            var res = _repository.TraerFabricante(codigo);
            if (res is null) return NotFound("El fabricante no existe.");
            return Ok(res);
        }

        [HttpPut]
        public IActionResult ActualizarFabricante([FromBody] Fabricante fabricante)
        {
            if (fabricante is null) return BadRequest("El fabricante es Nulo");
            var res = _repository.ActualizarFabricante(fabricante);
            if (!res) return UnprocessableEntity("El fabricante no se pudo actualizar. Verifica que no exista ya.");
            //return Ok("El nombre del fabricante con ID: "+fabricante.Codigo+" se actualizo correctamente a "+fabricante.Nombre);
            return Ok($"El nombre del fabricante con ID:'{fabricante.Codigo}' se actualizo corractamente a '{fabricante.Nombre}'");
        }
        [HttpDelete("{codigo}")]
        public IActionResult BorrarFabricante(uint codigo)
        {
            var res = _repository.EliminarFabricante(codigo);
            if (!res) return UnprocessableEntity("El fabricante no se pudo borrar. Es posible que no exista o que tenga productos asociados.");
            return Ok("El fabricante ha sido borrado.");
        }
        [HttpPost]
        public IActionResult AgregarFabricante([FromBody]Fabricante fabricante)
        {
            if (fabricante is null) return BadRequest("El fabricante es Nulo");
            var res = _repository.AgregarFabricante(fabricante);
            if (res == 0) return BadRequest("No se pudo agregar el fabricante.");
            return Ok(new { Message = "Se agrego correctamente", NewId = res });
        }
    }
}
