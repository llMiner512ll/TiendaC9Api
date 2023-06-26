using System;
using System.Collections.Generic;

namespace TiendaApi.Models;

public partial class Producto
{
    public uint Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public double Precio { get; set; }

    public uint CodigoFabricante { get; set; }

    public virtual Fabricante? CodigoFabricanteNavigation { get; set; }
}
