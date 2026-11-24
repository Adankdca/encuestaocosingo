using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Encuestasopm.Modelos
{
    public class combos
    {
        public String CadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["BDLOCALonnectionString"].ConnectionString;
        public SqlConnection conexion;
        public DataTable CargarplanprogramaBD()
        {
            DataTable dt = null;
            try
            {
                AbrirConexion();

                SqlDataAdapter da = new SqlDataAdapter("SPObtenerplanprograma", conexion);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Clear();
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception) { }
            finally { CerrarConexion(); }

            return dt;
        }
        public DataTable CargarEntidadBD()
        {
            DataTable dt = null;
            try
            {
                AbrirConexion();

                SqlDataAdapter da = new SqlDataAdapter("SPObtenerEntidad", conexion);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Clear();
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception) { }
            finally { CerrarConexion(); }

            return dt;
        }
        public DataTable CargarOcupacionBD()
        {
            DataTable dt = null;
            try
            {
                AbrirConexion();

                SqlDataAdapter da = new SqlDataAdapter("SPObtenerOcupacion", conexion);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Clear();
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception) { }
            finally { CerrarConexion(); }

            return dt;
        }
        public DataTable CargarDependenciaBD()
        {
            DataTable dt = null;
            try
            {
                AbrirConexion();

                SqlDataAdapter da = new SqlDataAdapter("SPObtenerDependencia", conexion);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Clear();
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception) { }
            finally { CerrarConexion(); }

            return dt;
        }
        public String EnviarDenunciaBD(String anonimo, String notificar, String nombre, String direccion, String telefono, String email,
            String entidadocurre, String fechaini, String fechafin, String narracion, String nomdenunciado, String patdenunciado,
            String matdenunciado, String dependencia, String adscripcion, String puesto, String entidaddenunciado, String nomconsta,
            String dirconsta, String telconsta, String emailconsta, String servpublico, String depenconsta, String puestoconsta)
        {
            String result = "";
            try
            {
                AbrirConexion();
                SqlCommand dc = new SqlCommand("Insert_tblc_quejasdenuncias", conexion);
                dc.CommandType = CommandType.StoredProcedure;
                dc.Parameters.Clear();
                dc.Parameters.AddWithValue("@anonimo", Convert.ToInt32(anonimo));
                dc.Parameters.AddWithValue("@notificar", Convert.ToInt32(notificar));
                dc.Parameters.AddWithValue("@nombre", nombre);
                dc.Parameters.AddWithValue("@direccion", direccion);
                dc.Parameters.AddWithValue("@telefono", telefono);
                dc.Parameters.AddWithValue("@email", email);
                dc.Parameters.AddWithValue("@entidadocurre", Convert.ToInt32(entidadocurre));
                dc.Parameters.AddWithValue("@fechaini", Convert.ToDateTime(fechaini));
                dc.Parameters.AddWithValue("@fechafin", Convert.ToDateTime(fechafin));
                dc.Parameters.AddWithValue("@narracion", narracion);
                dc.Parameters.AddWithValue("@nomdenunciado", nomdenunciado);
                dc.Parameters.AddWithValue("@patdenunciado", patdenunciado);
                dc.Parameters.AddWithValue("@matdenunciado", matdenunciado);
                dc.Parameters.AddWithValue("@dependencia", Convert.ToInt32(dependencia));
                dc.Parameters.AddWithValue("@adscripcion", adscripcion);
                dc.Parameters.AddWithValue("@puesto", puesto);
                dc.Parameters.AddWithValue("@entidaddenunciado", Convert.ToInt32(entidaddenunciado));
                dc.Parameters.AddWithValue("@nomconsta", nomconsta);
                dc.Parameters.AddWithValue("@dirconsta", dirconsta);
                dc.Parameters.AddWithValue("@telconsta", telconsta);
                dc.Parameters.AddWithValue("@emailconsta", emailconsta);
                dc.Parameters.AddWithValue("@servpublico", servpublico);
                dc.Parameters.AddWithValue("@depenconsta", Convert.ToInt32(depenconsta));
                dc.Parameters.AddWithValue("@puestoconsta", puestoconsta);
                SqlParameter param = new SqlParameter("@result", SqlDbType.VarChar, 30);
                param.Direction = ParameterDirection.Output;
                dc.Parameters.Add(param);
                dc.ExecuteNonQuery();
                result = param.Value.ToString();
            }
            catch (Exception ex)
            {
                result = "EI";
            }
            finally { CerrarConexion(); }
            return result;
        }
        public DataTable cargarBandejaBD()
        {
            DataTable dt = null;
            try
            {
                AbrirConexion();
                SqlDataAdapter da = new SqlDataAdapter("SPobtenerquejas", conexion);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Clear();
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception) { }
            finally { CerrarConexion(); }

            return dt;
        }
        public DataTable quejaDetalleBD(String idqueja)
        {
            DataTable dt = null;
            try
            {
                AbrirConexion();
                SqlDataAdapter da = new SqlDataAdapter("SPquejaDetalle", conexion);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Clear();
                da.SelectCommand.Parameters.AddWithValue("@idqueja", Convert.ToInt32(idqueja));
                da.SelectCommand.ExecuteNonQuery();
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception ex) { }
            finally { CerrarConexion(); }
            return dt;
        }

        //========================================================================================================================
        //===================================ENCUESTA=============================================================================
        //========================================================================================================================
        public String EnviarencuestaBD(String txtnomciudadano, String cbogenero, String radioservidoredad, String cboocupacion, String cbonacido, String idplanes, String cboexistencia, String cboapoyo, String txtproblematica, String txtpropuesta, String inseguridad, String bienestar, String abandono, String economico, String desatencion, String archivo, String cboconfianza)
        {
            String result = "";
            try
            {
                AbrirConexion();
                SqlCommand dc = new SqlCommand("Insert_Encuesta", conexion);
                dc.CommandType = CommandType.StoredProcedure;
                dc.Parameters.Clear();
                dc.Parameters.AddWithValue("@idplanes", Convert.ToInt32(idplanes));
                dc.Parameters.AddWithValue("@Nombre", txtnomciudadano);
                dc.Parameters.AddWithValue("@Genero", cbogenero);
                dc.Parameters.AddWithValue("@idedad", Convert.ToInt32(radioservidoredad));
                dc.Parameters.AddWithValue("@Ocupacion", cboocupacion);
                dc.Parameters.AddWithValue("@nacido", cbonacido);
                dc.Parameters.AddWithValue("@existencia", cboexistencia);
                dc.Parameters.AddWithValue("@apoyo", cboapoyo);
                dc.Parameters.AddWithValue("@inseguridad", Convert.ToInt32(inseguridad));
                dc.Parameters.AddWithValue("@bienestar", Convert.ToInt32(bienestar));
                dc.Parameters.AddWithValue("@abandono", Convert.ToInt32(abandono));
                dc.Parameters.AddWithValue("@economico", Convert.ToInt32(economico));
                dc.Parameters.AddWithValue("@desatencion", Convert.ToInt32(desatencion));
                dc.Parameters.AddWithValue("@problematica", txtproblematica);
                dc.Parameters.AddWithValue("@propuesta", txtpropuesta);
                dc.Parameters.AddWithValue("@archivo", archivo);
                dc.Parameters.AddWithValue("@idconfianza", Convert.ToInt32(cboconfianza));
                dc.Parameters.AddWithValue("@fechaencuesta", DateTime.Now);
                SqlParameter param = new SqlParameter("@result", SqlDbType.VarChar, 30);
                param.Direction = ParameterDirection.Output;
                dc.Parameters.Add(param);
                dc.ExecuteNonQuery();
                result = param.Value.ToString();
            }
            catch (Exception ex)
            {
                result = "FALLO";
            }
            finally { CerrarConexion(); }
            return result;
        }

        //========================================================================================================================
        //===================================ENCUESTA=============================================================================
        //========================================================================================================================

        #region Metodos Generales

        public Boolean AbrirConexion()
        {
            Boolean exito;
            try
            {
                conexion = new SqlConnection(CadenaConexion);
                conexion.Open();
                exito = true;
            }
            catch (Exception)
            {
                exito = false;
            }
            return exito;
        }
        public void CerrarConexion()
        {
            if (conexion == null) { return; }
            conexion.Close();
            conexion = null;
        }


        #endregion
    }
    public class planesprogramas
    {
        public String idplanes { get; set; }
        public String nombre { get; set; }
    }
    public class entidadocurreClass
    {
        public String idMunOcurre { get; set; }
        public String munocurre { get; set; }
    }
    public class entidaddenunciadoClass
    {
        public String idMunDenunciado { get; set; }
        public String mundenunciado { get; set; }
    }
    public class dependenciaconstaClass
    {
        public String idDepenConsta { get; set; }
        public String depenconsta { get; set; }
    }
    public class dependenciadenunciadoClass
    {
        public String idDepenDenunciado { get; set; }
        public String Dependenunciado { get; set; }
    }
    public class bandejaquejasClass
    {
        public String idqueja { get; set; }
        public String nombre { get; set; }
        public String telefono { get; set; }
        public String email { get; set; }
        public String nomservidor { get; set; }
        public String depenservidor { get; set; }
        public String adscripcion { get; set; }
        public String hechos { get; set; }
        public String idstatus { get; set; }
        public String status { get; set; }
        public String fechaini { get; set; }
        public String fechafin { get; set; }
    }
    public class quejaDetalleClass
    {
        public String idqueja { get; set; }
        public String folioqueja { get; set; }
        public String tipo { get; set; }
        public String notificar { get; set; }
        public String nomvic { get; set; }
        public String direccionvic { get; set; }
        public String telefonovic { get; set; }
        public String emailvic { get; set; }
        public String entidadhecho { get; set; }
        public String fechaini { get; set; }
        public String fechafin { get; set; }
        public String narracion { get; set; }
        public String nombrecontra { get; set; }
        public String depcontra { get; set; }
        public String adscontra { get; set; }
        public String cargocontra { get; set; }
        public String entidadcontra { get; set; }
        public String nomconsta { get; set; }
        public String dirconsta { get; set; }
        public String telconsta { get; set; }
        public String emailconsta { get; set; }
        public String publico { get; set; }
        public String depenconsta { get; set; }
        public String cargoconsta { get; set; }
        public String estatus { get; set; }
        public String fechareg { get; set; }
    }
    public class resultado
    {
        public String respuesta { get; set; }

    }
}