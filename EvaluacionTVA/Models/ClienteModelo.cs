using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EvaluacionTVA.Models
{
    public class ClienteModelo
    {
        [Column("CLIENTE_ID")]
        public int ClienteId { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("CLAVE")]
        public string Clave { get; set; }
        [Column("MAIL")]
        public string Mail { get; set; }
        [Column("ESTATUS")]
        public string Estatus { get; set; }
    }
}