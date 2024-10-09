using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvaluacionTVA.Models
{
    public class VistaComprasPorCliente
    {
        public string ClaveCliente { get; set; }
        public int VentaId { get; set; }
        public string Cliente { get; set; }
        public string Mail { get; set; }
        public DateTime FechaVenta { get; set; }
        public double TotalVenta { get; set; }
        public string DetalleUrl { get; set; } // Enlace para el modal de detalle de la venta
    }
}