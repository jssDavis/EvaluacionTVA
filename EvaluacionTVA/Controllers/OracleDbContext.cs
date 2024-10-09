using System;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;

namespace EvaluacionTVA.Controllers
{
    public class OracleDbContext
    {
        private readonly string _ConnectionString;

        public OracleDbContext() {
            _ConnectionString = ConfigurationManager.ConnectionStrings["OracleDB"].ConnectionString;
        }

        public OracleConnection GetConnection() {

            try
            {
                var conn = new OracleConnection(_ConnectionString);
                conn.Open();
                return conn;
            } catch ( Exception ex ) {
                Console.WriteLine("Error al conectar a DB: " +  ex.Message );
                return null;
            }

        }

        public void CloseConnection( OracleConnection conn ) {

            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }


    }
}