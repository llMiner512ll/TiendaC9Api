using System;
using System.Collections.Generic;

namespace TiendaApi.Models;

public partial class Fabricante
{
    public uint Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
