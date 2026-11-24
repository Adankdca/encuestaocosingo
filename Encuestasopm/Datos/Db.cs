using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Encuestasopm.Datos
{
    /// <summary>
    /// Acceso mínimo a SQL Server. Todo pasa por procedimientos almacenados con parámetros
    /// (sin concatenar SQL) y cada llamada abre y cierra su propia conexión (pool de ADO.NET).
    /// </summary>
    public static class Db
    {
        private static string Cadena
        {
            get { return ConfigurationManager.ConnectionStrings["EncuestaDB"].ConnectionString; }
        }

        public static DataSet Consultar(string procedimiento, params SqlParameter[] parametros)
        {
            using (var cn = new SqlConnection(Cadena))
            using (var cmd = Crear(cn, procedimiento, parametros))
            using (var da = new SqlDataAdapter(cmd))
            {
                var ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public static void Ejecutar(string procedimiento, params SqlParameter[] parametros)
        {
            using (var cn = new SqlConnection(Cadena))
            using (var cmd = Crear(cn, procedimiento, parametros))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static SqlCommand Crear(SqlConnection cn, string procedimiento, SqlParameter[] parametros)
        {
            var cmd = new SqlCommand(procedimiento, cn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 60 };
            if (parametros != null)
            {
                foreach (var p in parametros)
                {
                    if (p.Value == null) p.Value = DBNull.Value;
                    cmd.Parameters.Add(p);
                }
            }
            return cmd;
        }

        public static SqlParameter P(string nombre, SqlDbType tipo, object valor, int tamano = 0)
        {
            var p = tamano > 0 ? new SqlParameter(nombre, tipo, tamano) : new SqlParameter(nombre, tipo);
            p.Value = valor ?? DBNull.Value;
            return p;
        }

        public static SqlParameter Salida(string nombre, SqlDbType tipo, int tamano = 0)
        {
            var p = tamano > 0 ? new SqlParameter(nombre, tipo, tamano) : new SqlParameter(nombre, tipo);
            p.Direction = ParameterDirection.Output;
            return p;
        }

        /// <summary>Convierte una tabla en una lista serializable a JSON (fechas en formato ISO).</summary>
        public static List<Dictionary<string, object>> ALista(DataTable dt)
        {
            var lista = new List<Dictionary<string, object>>(dt.Rows.Count);
            foreach (DataRow r in dt.Rows)
            {
                var fila = new Dictionary<string, object>(dt.Columns.Count);
                foreach (DataColumn c in dt.Columns)
                {
                    object v = r[c];
                    if (v == DBNull.Value) v = null;
                    else if (v is DateTime) v = ((DateTime)v).ToString("yyyy-MM-ddTHH:mm:ss");
                    else if (v is byte[]) continue;
                    fila[c.ColumnName] = v;
                }
                lista.Add(fila);
            }
            return lista;
        }
    }
}
