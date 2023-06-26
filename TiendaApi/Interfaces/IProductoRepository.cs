using Microsoft.EntityFrameworkCore;
using TiendaApi.Data;
using TiendaApi.Models;

namespace TiendaApi.Interfaces
{
    public interface IProductoRepository
    {
        /// <summary>
        /// Este metodo trae a todos los productos.
        /// </summary>
        /// <returns>Productos.</returns>
        IEnumerable<Producto> TraerProductos();
        /// <summary>
        /// Este metodo trae a un producto por su ID.
        /// </summary>
        /// <returns>Producto.</returns>
        Producto? TraerProducto(uint codigo);
        /// <summary>
        /// Elimina el producto por su id.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns>True: Se elimino exitosamente. False: Hubo un error o no se existia para ser eliminado.</returns>
        bool EliminarProducto(uint codigo);
        /// <summary>
        /// Este metodo actualiza un producto con los valores dados.
        /// </summary>
        /// <param name="producto"></param>
        /// <returns>True si se actualizo correctamente. False si el nombre existe o ocurrio un error.
        /// </returns>
        bool ActualizarProducto(Producto producto);
        /// <summary>
        /// Este metodo agrega un producto con los valores dados.
        /// </summary>
        /// <param name="producto"></param>
        /// <returns>True si se agrego correctamente. False si ocurrio un error.</returns>
        uint AgregarProducto(Producto producto);
    }
    public class ProductoRepository : IProductoRepository
    {
        private readonly Tienda9CDbContext _context;
        public ProductoRepository(Tienda9CDbContext context)
        {
            _context = context;
        }
        public bool ActualizarProducto(Producto producto)
        {
            try
            {
                var current = _context.Productos.FirstOrDefault(x => x.Codigo == producto.Codigo);
                if (current is null) return false;
                current.Nombre = producto.Nombre;
                current.Precio = producto.Precio;
                current.CodigoFabricante = producto.CodigoFabricante;
                _context.Productos.Update(current);
                int rows = _context.SaveChanges();
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public uint AgregarProducto(Producto producto)
        {
            try
            {
                _context.Add(producto);
                _context.SaveChanges();
                return producto.Codigo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public bool EliminarProducto(uint codigo)
        {
            try
            {
                var current = _context.Productos.FirstOrDefault(x => x.Codigo == codigo);
                if (current is null) return false;
                _context.Productos.Remove(current);
                var rows = _context.SaveChanges();
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public Producto? TraerProducto(uint codigo)
        {
            try
            {
                return _context.Productos.Include(x => x.CodigoFabricanteNavigation).FirstOrDefault(x => x.Codigo == codigo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public IEnumerable<Producto> TraerProductos()
        {
            try
            {
                return _context.Productos.Include(x => x.CodigoFabricanteNavigation).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Enumerable.Empty<Producto>();
            }
        }
    }
}
