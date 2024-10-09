using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvaluacionTVA.Models
{
    public class DetalleVentaModelo
    {
        public int VentaId { get; set; }        // VENTA_ID
        public int ProductoId { get; set; }     // PRODUCTO_ID
        public int Cantidad { get; set; }       // CANTIDAD
        public decimal Descuento { get; set; }  // DESCUENTO
        public decimal Total { get; set; }      // TOTAL
    }
}