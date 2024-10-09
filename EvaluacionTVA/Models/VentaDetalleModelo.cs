using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvaluacionTVA.Models
{
    public class VentaDetalleModelo
    {
        public int VentaId { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public double CostoUnitario { get; set; }
        public double Total { get; set; }
        public int ProductoId { get; set; }
    }
}