using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvaluacionTVA.Models
{
    public class NuevaVentaModelo
    {
        public int ClienteId { get; set; }
        public List<VentaDetalleModelo> Detalles { get; set; }
    }
}