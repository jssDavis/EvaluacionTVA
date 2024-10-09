using EvaluacionTVA.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace EvaluacionTVA.Controllers
{
    public class VentaController : Controller
    {
        private readonly OracleDbContext _dbcontext;

        public VentaController() { 
        
            _dbcontext = new OracleDbContext();
        }


        // GET: VentasControlador
        public ActionResult Index(string nombre = null, string clave = null, string estatus = null)
        {

            var ventas = ObtenerCompras(nombre,clave,estatus);
            return View(ventas);
        }


        // Acción para abrir el modal de agregar venta
        public ActionResult AgregarVentaModal()
        {
            var productos = ObtenerProductos();
            ViewBag.Clientes = new SelectList(ObtenerClientes(), "ClienteID", "Nombre");
            ViewBag.Productos = new SelectList(productos, "ProductoId", "Descripcion");

            // Enviar datos de los productos como JSON
            ViewBag.ProductosData = productos.Select(p => new
            {
                p.ProductoId,
                p.Descripcion,
                p.CostoUnitario
            }).ToList();

            return PartialView("_AgregarVentaModal");
        }










        // Acción para mostrar el detalle de una venta en un modal
        public ActionResult Detalle(int ventaId)
        {

            var detalles = new List<VentaDetalleModelo>();


            using (OracleConnection conn = _dbcontext.GetConnection())
            {
                using (var cmd = new OracleCommand($"Select * from VISTA_DETALLE_VENTA where VENTA_ID={ventaId}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) {
                             var detalle = new VentaDetalleModelo {
                                 VentaId = Convert.ToInt32(reader["VENTA_ID"]),
                                 Producto = reader["PRODUCTO"].ToString(),
                                 Cantidad = Convert.ToInt32(reader["CANTIDAD"]),
                                 CostoUnitario = Convert.ToDouble(reader["COSTO_UNITARIO"]),
                                 Total = Convert.ToDouble(reader["TOTAL"])
                             };
                           
                            
                           detalles.Add(detalle);

                        }

                        
                    }
                }
            }

                       
            return (PartialView("_DetalleModal", detalles));
        }


        private IEnumerable<VistaComprasPorCliente> ObtenerCompras(string nombre = null, string clave = null, string estatus = null) {
            


            List<VistaComprasPorCliente> compras = new List<VistaComprasPorCliente>();

            
            var query = new StringBuilder("SELECT * FROM VISTA_COMPRAS_POR_CLIENTE WHERE 1=1"); 
                                                                                               
            if (!string.IsNullOrEmpty(nombre))
            {
                query.Append($" AND CLIENTE LIKE  '%{nombre }%'");
            }

            if (!string.IsNullOrEmpty(clave))
            {
                query.Append($" AND CLAVE_CLIENTE LIKE  '%{clave}%'");
            }

            if (!string.IsNullOrEmpty(estatus))
            {
                query.Append($" AND ESTATUS = {estatus}");
            }






            using (OracleConnection conn = _dbcontext.GetConnection() ) {
                using ( var cmd = new OracleCommand (query.ToString(), conn) ) {
                    using ( var reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            var compra = new VistaComprasPorCliente
                            {
                                ClaveCliente = reader["CLAVE_CLIENTE"] != DBNull.Value ? Convert.ToString(reader["CLAVE_CLIENTE"]) : "-" ,
                                VentaId = reader["VENTAID"] != DBNull.Value ? Convert.ToInt32(reader["VENTAID"]) : 0,
                                Cliente = reader["CLIENTE"] != DBNull.Value ? reader["CLIENTE"].ToString() : "-",
                                Mail = reader["MAIL"] != DBNull.Value ? reader["MAIL"].ToString(): "@",
                                FechaVenta = reader["FECHA_VENTA"] != DBNull.Value ? Convert.ToDateTime(reader["FECHA_VENTA"]) : DateTime.MinValue,
                                TotalVenta = reader["TOTAL_VENTA"] != DBNull.Value ? Convert.ToDouble(reader["TOTAL_VENTA"]) : 0                             
                            };

                            compras.Add(compra);
                        }
                    }
                }
            }

            return compras;
        }

        // Método para obtener clientes activos de Oracle
        private List<ClienteModelo> ObtenerClientes()
        {
            List<ClienteModelo> clientes= new List<ClienteModelo>();
            using (var conn = _dbcontext.GetConnection())
            {
                using (var cmd = new OracleCommand("SELECT * FROM CLIENTE WHERE ESTATUS = 'ACTIVO'", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ClienteModelo cliente = new ClienteModelo
                            {
                                ClienteId = reader["CLIENTE_ID"] != DBNull.Value ? Convert.ToInt32(reader["CLIENTE_ID"]) : 0,
                                Nombre = reader["NOMBRE"] != DBNull.Value ? Convert.ToString(reader["NOMBRE"]) : "",
                                Clave = reader["CLAVE"] != DBNull.Value ? Convert.ToString(reader["CLAVE"]) : "",
                                Mail = reader["MAIL"] != DBNull.Value ? Convert.ToString(reader["MAIL"]) : "",
                                Estatus = reader["ESTATUS"] != DBNull.Value ? Convert.ToString(reader["ESTATUS"]) : "",
                            };
                            clientes.Add(cliente);
                        }
                    }
                }
            }
            return clientes;
        }


        // Método para obtener Productos activos de Oracle
        private List<ProductoModelo> ObtenerProductos()
        {
            List<ProductoModelo> productos = new List<ProductoModelo>();
            using (var conn = _dbcontext.GetConnection())
            {
                using (var cmd = new OracleCommand("SELECT * FROM PRODUCTO WHERE ESTATUS = 'ACTIVO'", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductoModelo producto = new ProductoModelo
                            {
                                ProductoId = reader["PRODUCTO_ID"] != DBNull.Value ? Convert.ToInt32(reader["PRODUCTO_ID"]) : 0,
                                Descripcion = reader["DESCRIPCION"] != DBNull.Value ? Convert.ToString(reader["DESCRIPCION"]) : "",
                                CostoUnitario = reader["COSTO_UNITARIO"] != DBNull.Value ? Convert.ToDouble(reader["COSTO_UNITARIO"]) : 0,
                                Estatus = reader["ESTATUS"] != DBNull.Value ? Convert.ToString(reader["ESTATUS"]) : "",

                            };
                            productos.Add(producto);
                        }
                    }
                }
            }
            return productos;
        }




        // Acción para guardar una nueva venta
        [HttpPost]
        public ActionResult AgregarVenta(NuevaVentaModelo model)
        {
            var nuevaVenta = new VentaModelo();
            if (ModelState.IsValid)
            {
                // Guardar la venta y detalles
                 nuevaVenta = new VentaModelo
                {

                    ClienteId = model.ClienteId,
                    Fecha = DateTime.Now,
                    Estatus = "Pendiente",
                    Detalles = new List<VentaDetalleModelo>()

                };
                /*db.Ventas.Add(nuevaVenta);
                db.SaveChanges();*/

                foreach (var detalle in model.Detalles)
                {
                    nuevaVenta.Detalles.Add( new VentaDetalleModelo
                    {
  
                        ProductoId = detalle.ProductoId,
                        Cantidad = detalle.Cantidad,
                        CostoUnitario = detalle.CostoUnitario,
                        Total = detalle.Total
                    });
                    /*db.DetallesVenta.Add(nuevoDetalle);*/
                }
                /*db.SaveChanges();
                return RedirectToAction("Index");*/
            }

            //Almacenar en Base 
            try
            {
                // Guardar la venta
                using (var conn = _dbcontext.GetConnection())
                {
                    
                    

                    // Primero inserta la venta
                    using (var cmd = new OracleCommand("INSERT INTO VENTA (VENTA_ID, FECHA, CLIENTE_ID, ESTATUS) VALUES (SEQ_VENTA.NEXTVAL, :fecha, :clienteId, :estatus)", conn))
                    {
                        cmd.Parameters.Add(new OracleParameter(":fecha", nuevaVenta.Fecha));
                        cmd.Parameters.Add(new OracleParameter(":clienteId", nuevaVenta.ClienteId));
                        cmd.Parameters.Add(new OracleParameter(":estatus", nuevaVenta.Estatus));
                        cmd.ExecuteNonQuery();
                    }

                    // Obtener el ID de la venta recién creada
                    // Esto depende de cómo estés manejando la generación del ID en tu base de datos.
                    // Asegúrate de que VENTA_ID se genere automáticamente.

                    int ventaId = ObtenerUltimoIdVenta();
                    // Luego inserta los detalles
                    foreach (var detalle in nuevaVenta.Detalles)
                    {
                        using (var cmd = new OracleCommand("INSERT INTO DETALLE_VENTA (VENTA_ID, PRODUCTO_ID, CANTIDAD,  TOTAL) VALUES (:ventaId, :productoId, :cantidad, :total)", conn))
                        {
                            cmd.Parameters.Add(new OracleParameter(":ventaId", ventaId));
                            cmd.Parameters.Add(new OracleParameter(":productoId", detalle.ProductoId));
                            cmd.Parameters.Add(new OracleParameter(":cantidad", detalle.Cantidad));
                            cmd.Parameters.Add(new OracleParameter(":total", detalle.Total));

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                return RedirectToAction("Index"); // Redirigir a la acción deseada
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ModelState.AddModelError("", "Error al guardar la venta: " + ex.Message);
                return View(model); // Regresa la vista con el modelo para mostrar errores
                throw;
            }


            // Si hay errores, devolver el modelo para corregir
            //ViewBag.Clientes = new SelectList(db.Clientes.Where(c => c.Estatus == "ACTIVO"), "ClienteId", "Nombre");
            //ViewBag.Productos = new SelectList(db.Productos.Where(p => p.Estatus == "ACTIVO"), "ProductoId", "Descripcion");
            //return PartialView("_AddSaleModal", model);
        }


        public int ObtenerUltimoIdVenta()
        {
            int ultimoIdVenta = 0; 
            using (var conn = _dbcontext.GetConnection())
            {
         


                using (var cmd = new OracleCommand("SELECT MAX(VENTA_ID) FROM VENTA", conn))
                {
                    var result = cmd.ExecuteScalar(); 
                    if (result != DBNull.Value)
                    {
                        ultimoIdVenta = Convert.ToInt32(result);
                    }
                }
            }
            return ultimoIdVenta; // Devuelve el último ID de venta
        }

    }
}
