using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EvaluacionTVA.Models
{
    public class VentaModelo
    {
        [Column("VENTA_ID")]
        public int VentaId { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
        [Column("CLIENTE_ID")]
        public int ClienteId { get; set; }
        [Column("ESTATUS")]
        public string Estatus { get; set; }
        public List<VentaDetalleModelo> Detalles { get; set; }
    }
}