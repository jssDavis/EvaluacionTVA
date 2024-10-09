using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EvaluacionTVA.Models
{
    public class ProductoModelo
    {
        [Column("PRODUCTO_ID")]
        public int ProductoId { get; set; }
        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }
        [Column("COSTO_UNITARIO")]
        public double CostoUnitario { get; set; }
        [Column("ESTATUS")]
        public string Estatus { get; set; }
    }
}