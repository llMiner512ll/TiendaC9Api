using Microsoft.EntityFrameworkCore;
using TiendaApi.Data;
using TiendaApi.Models;

namespace TiendaApi.Interfaces
{
    public interface IFabricanteRepository
    {
        /// <summary>
        /// Este metodo trae a todos los fabricantes.
        /// </summary>
        /// <returns>Fabricantes.</returns>
        IEnumerable<Fabricante> TraerFabricantes();
        /// <summary>
        /// Este metodo trae a un fabricante por su ID.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns>Un fabricante.</returns>
        Fabricante? TraerFabricante(uint codigo);
        /// <summary>
        /// Si el fabricante existe y no tiene productos que lo referencien.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns>True: Se elimino exitosamente. False: Hubo un error o no se cumplian las condiciones mencionadas para ser eliminado.</returns>
        bool EliminarFabricante(uint codigo);
        /// <summary>
        /// Este metodo actualiza un fabricante con los valores dados.
        /// </summary>
        /// <param name="fabricante"></param>
        /// <returns>True si se actualizo correctamente. False si el nombre existe o ocurrio un error.
        /// </returns>
        bool ActualizarFabricante(Fabricante fabricante);
        /// <summary>
        /// Este metodo agrega un fabricante con los valores dados.
        /// </summary>
        /// <param name="fabricante"></param>
        /// <returns>True si se agrego correctamente. False si el fabricante existe o ocurrio un error.</returns>
        uint AgregarFabricante(Fabricante fabricante);

    }
    public class FabricanteRepository : IFabricanteRepository
    {
        private readonly Tienda9CDbContext _context;
        public FabricanteRepository (Tienda9CDbContext context)
        {
            _context = context;
        }
        
        public bool ActualizarFabricante(Fabricante fabricante)
        {
            try
            {
                var current = _context.Fabricantes.FirstOrDefault(x => x.Codigo==fabricante.Codigo);
                if (current is null) return false;
                if (_context.Fabricantes.Any(f => f.Nombre==fabricante.Nombre&&f.Codigo!=fabricante.Codigo)) return false;
                current.Nombre = fabricante.Nombre;
                _context.Fabricantes.Update(current);
                int rows = _context.SaveChanges();
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public uint AgregarFabricante(Fabricante fabricante)
        {
            try
            {
                if (_context.Fabricantes.Any(f => f.Nombre == fabricante.Nombre)) return 0;
                _context.Add(fabricante);
                _context.SaveChanges();
                return fabricante.Codigo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }


        public bool EliminarFabricante(uint codigo)
        {
            try
            {
                var haveChild = _context.Fabricantes.Any(x => x.Codigo == codigo && x.Productos.Any());
                if (haveChild) return false;
                var current = _context.Fabricantes.FirstOrDefault(x => x.Codigo == codigo);
                if (current is null) return false;
                _context.Fabricantes.Remove(current);
                var rows = _context.SaveChanges();
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public IEnumerable<Fabricante> TraerFabricantes()
        {
            try
            {
                return _context.Fabricantes.Include(x => x.Productos).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Enumerable.Empty<Fabricante>();
            }
        }
        public Fabricante? TraerFabricante(uint codigo)
        {
            try
            {
                return _context.Fabricantes.Include(x => x.Productos).FirstOrDefault(x => x.Codigo == codigo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
