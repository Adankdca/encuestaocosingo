using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI;

namespace Encuesta
{
    public class Classconexion
    {

    }
    public class SelectCommand1
    {
        public DataTable Table;
        public bool Ejecutado;
        public string MsjError;
        public SelectCommand1()
        {
            Table = null;
            Ejecutado = false;
            MsjError = "";
        }
    }
    public class SelectCommand2
    {
        public string Dato;
        public bool Ejecutado;
        public string MsjError;
        public SelectCommand2()
        {
            Ejecutado = false;
            Dato = "";
            MsjError = "";
        }
    }
    public class ExecCommand
    {
        public int RowAffected;
        public bool Ejecutado;
        public string MsjError;
        public string IDNew;


        public ExecCommand()
        {
            RowAffected = 0;
            Ejecutado = false;
            MsjError = "";
        }
    }
    public class StrCadena
    {
        public string _StrCadena;
        public StrCadena()
        {
            _StrCadena = "";
        }
    }
    public class Cnn
    {
        public string idgeneralNew;
        public int IDNewx;
        public Int64 IDNewtwitter;
        public string idobraccion;
        string StrCad;
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataAdapter Adapter;
        /// <summary>
        /// Objeto conexion
        /// </summary>
        public Cnn()
        {

            StrCad = @"data source=localhost; initial catalog=BDPlanesprogramas; persist security info=False;user id=sistema; password=sistema; workstation id=ADMIN;packet size=24096;";
            //StrCad = @"data source=SOPYC\SOPYC; initial catalog=BDSICOFIN; persist security info=False;user id=accesobd; password=accesobd; workstation id=ADMIN;packet size=24096;";
            //  StrCad = @"data source=SERVIDOR; initial catalog=BDSICOFIN;Integrated Security=SSPI; persist security info=True;";

            cnn = new SqlConnection(StrCad); cnn = new SqlConnection(StrCad);
            cmd = new SqlCommand("", cnn);
            Adapter = new SqlDataAdapter("", cnn);
        }
        public string conec()
        {
            //  string CadenaConexion = @"data source=SERVIDOR; initial catalog=BDSICOFIN;Integrated Security=SSPI; persist security info=True;";
            //string CadenaConexion = @"data source=SOPYC\SOPYC; initial catalog=BDSICOFIN; persist security info=False;user id=accesobd; password=accesobd; workstation id=ADMIN;packet size=24096;";
            string CadenaConexion = @"data source=localhost; initial catalog=BDPlanesprogramas; persist security info=False;user id=sistema; password=sistema; workstation id=ADMIN;packet size=24096;";
            return CadenaConexion;
        }


        /// <summary>
        /// Ejecuta una consulta y devuelve el resultado como una clase
        /// </summary>
        /// <param name="SQL">SQL a ajecutar</param>
        /// <returns>Tbl: DataTable, MsjError: mensaje de error</returns>
        public SelectCommand1 SelectCmd1(string SQL)
        {
            SelectCommand1 rs = new SelectCommand1();
            DataSet ds = new DataSet();
            try
            {
                cnn.Open();
                Adapter.SelectCommand.CommandText = SQL;
                Adapter.Fill(ds);
                rs.Table = ds.Tables[0];
                rs.Ejecutado = true;
                return rs;
            }
            catch (Exception e)
            {
                rs.Table = null;
                rs.Ejecutado = false;
                rs.MsjError = e.Message;
                return rs;
            }
            finally
            {
                cnn.Close();
            }
        }
        public StrCadena _StrCad_()
        {
            StrCadena _StrCad = new StrCadena();//cadena de conexión      
            _StrCad._StrCadena = StrCad.ToString();
            return _StrCad;
        }
        /// <summary>
        /// Ejecuta una consulta y devuelve el resultado como una clase
        /// </summary>
        /// <param name="SQL">SQL a ajecutar</param>
        /// <returns>string: resultado</returns>
        public SelectCommand2 SelectCmd2(string SQL)
        {
            SelectCommand2 rs = new SelectCommand2();
            DataSet ds = new DataSet();
            try
            {
                cnn.Open();
                Adapter.SelectCommand.CommandText = SQL;
                Adapter.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    rs.Dato = ds.Tables[0].Rows[0][0].ToString();
                }
                rs.Ejecutado = true;
                return rs;
            }
            catch (Exception e)
            {
                rs.Ejecutado = false;
                rs.Dato = "";
                rs.MsjError = e.Message;
                return rs;
            }
            finally
            {
                cnn.Close();
            }
        }
        /// <summary>
        /// Ejecuta la insercion, update o eliminacion de registros de una tabla.
        /// </summary>
        /// <param name="SQL">Sql a ejecutar</param>
        /// <returns>i: número de registros afectados, msgError: mensaje de error</returns>
        public ExecCommand ExecuteCmd(string SQL)
        {
            ExecCommand ins = new ExecCommand();
            try
            {
                cnn.Open();
                cmd.CommandText = SQL;
                cmd.CommandType = CommandType.Text;
                ins.RowAffected = cmd.ExecuteNonQuery();
                ins.Ejecutado = true;
                return ins;
            }
            catch (Exception e)
            {
                ins.RowAffected = -1;
                ins.Ejecutado = false;
                ins.MsjError = e.Message;
                return ins;
            }
            finally
            {
                cnn.Close();
            }
        }
        public ExecCommand ExecuteCmdScalar(string SQL)
        {
            ExecCommand ins = new ExecCommand();
            try
            {
                cnn.Open();
                cmd.CommandText = SQL;
                cmd.CommandType = CommandType.Text;
                ins.IDNew = Convert.ToString(cmd.ExecuteScalar());
                ins.Ejecutado = true;
                return ins;
            }
            catch (Exception e)
            {
                ins.IDNew = "0";
                ins.Ejecutado = false;
                ins.MsjError = e.Message;
                return ins;
            }
            finally
            {
                cnn.Close();
            }
        }
        /// <summary>
        /// Clase para llenar un ComboBox
        /// </summary>
        /// <param name="cmb">ComboBox que se va a rellenar</param>
        /// <param name="SQL">Sql a ejecutar</param>
        public void LlenaCombo(DropDownList cmb, string SQL)
        {
            DataSet ds = new DataSet();
            try
            {
                cnn.Open();
                Adapter.SelectCommand.CommandText = SQL;
                Adapter.Fill(ds);
                cmb.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                cmb.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                cmb.DataSource = ds.Tables[0];
                cmb.DataBind();
            }
            catch (Exception e)
            {

                //MessageBox.Show(e.Message, "Control Paciente", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                cnn.Close();
            }
        }

        public SelectCommand1 sp_documentos(String filtro)
        {
            SelectCommand1 rs = new SelectCommand1();
            DataSet ds = new DataSet();
            try
            {
                cnn.Open();
                //Crear un objeto DataAdapter y proporcionar el nombre del procedimiento almacenado. 
                Adapter = new SqlDataAdapter("sp_deudores", cnn);
                //Establecer el tipo de comando como StoredProcedure. 
                Adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Crear y agregar un parámetro a la colección Parameters del procedimiento almacenado. 
                Adapter.SelectCommand.Parameters.Add(new SqlParameter("@filtro", SqlDbType.NVarChar, 250));

                //Asignar el valor  a los parámetros. 
                Adapter.SelectCommand.Parameters["@filtro"].Value = filtro.ToString();

                Adapter.Fill(ds);
                rs.Table = ds.Tables[0];
                rs.Ejecutado = true;
                return rs;
            }
            catch (Exception e)
            {
                rs.Table = null;
                rs.Ejecutado = false;
                rs.MsjError = e.Message;
                return rs;
            }
            finally
            {
                cnn.Close();
            }
        }

        #region Procedimientos Almacenados Get Insert Update Delete


        public virtual void funInsert(SqlTransaction tx, string pNombreStored, string[,] pArreglo)
        {
            //'Creación de la función pública utilizada para llamar a todos los storeds
            //       ' pasando como parámetros el nombre del stored correspondiente y un 
            //       ' arreglo(nombre de la variable, tipo de dato, valor de la varible)



            try
            {

                if (pArreglo.GetUpperBound(1) > 0)
                {
                    int upper = pArreglo.GetUpperBound(0);
                    int lower = pArreglo.GetLowerBound(0);


                    SqlParameter[] sqlParams = new SqlParameter[pArreglo.GetUpperBound(0) + 1];
                    for (int ctr = lower; ctr <= upper; ctr++)
                    {
                        sqlParams[ctr] = new SqlParameter(pArreglo[ctr, 0].ToString(), TipoDato(pArreglo[ctr, 1]));
                        sqlParams[ctr].Value = pArreglo[ctr, 2];
                    }


                    //ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored, sqlParams));
                    IDNewx = (int)SqlHelper.ExecuteScalar(tx, CommandType.StoredProcedure, pNombreStored, sqlParams);
                }


            }
            catch (SqlException A)
            {
                throw new System.InvalidOperationException("Error de datos");
            }
            catch (Exception e)
            {
                throw new System.InvalidOperationException("Error de datos");

            }

        }
        public virtual void funUpdateDelete(SqlTransaction tx, string pNombreStored, string[,] pArreglo)
        {
            //'Creación de la función pública utilizada para llamar a todos los storeds
            //       ' pasando como parámetros el nombre del stored correspondiente y un 
            //       ' arreglo(nombre de la variable, tipo de dato, valor de la varible)

            try
            {

                if (pArreglo.GetUpperBound(1) > 0)
                {

                    SqlParameter[] sqlParams = new SqlParameter[pArreglo.GetUpperBound(0) + 1];
                    for (int ctr = pArreglo.GetLowerBound(0); ctr <= pArreglo.GetUpperBound(0); ++ctr)
                    {
                        sqlParams[ctr] = new SqlParameter(pArreglo[ctr, 0].ToString(), TipoDato(pArreglo[ctr, 1]));
                        sqlParams[ctr].Value = pArreglo[ctr, 2];
                    }



                    SqlHelper.ExecuteNonQuery(tx, CommandType.StoredProcedure, pNombreStored, sqlParams);
                }



            }
            catch (Exception e)
            {

                throw new System.InvalidOperationException("Error de datos");
            }


        }
        public virtual DataSet funGetbyStoresPropiosgrid(string pNombreStored, string[,] pArreglo)
        {
            //'Creación de la función pública utilizada para llamar a todos los storeds
            //       ' pasando como parámetros el nombre del stored correspondiente y un 
            //       ' arreglo(nombre de la variable, tipo de dato, valor de la varible)
            cnn.Open();

            DataSet ds = new DataSet("Set");
            try
            {

                if (pArreglo.GetUpperBound(1) > 0)
                {
                    int upper = pArreglo.GetUpperBound(1);
                    int lower = pArreglo.GetLowerBound(0);



                    SqlParameter[] sqlParams = new SqlParameter[pArreglo.GetUpperBound(0) + 1];
                    for (int ctr = pArreglo.GetLowerBound(0); ctr <= pArreglo.GetUpperBound(0); ++ctr)
                    {
                        sqlParams[ctr] = new SqlParameter(pArreglo[ctr, 0].ToString(), TipoDato(pArreglo[ctr, 1]));
                        sqlParams[ctr].Value = pArreglo[ctr, 2];
                    }

                    //'Cuando se pasan parametros
                    ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored, sqlParams));
                }
                else
                {
                    //'Para storeds que no necesitan pase de parametros
                    ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored));
                }

                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables.Count == 2)
                    {
                        ds.Tables[0].TableName = "ParentTabled";
                        ds.Tables[1].TableName = "ChildTabled";
                        ////// Set the table's primary key field
                        ds.Tables["ParentTabled"].PrimaryKey = new DataColumn[] {
                    ds.Tables["ParentTabled"].Columns[0] };
                        ds.AcceptChanges();
                        ds.Relations.Add(new DataRelation("relacion", ds.Tables[0].Columns[0], ds.Tables[1].Columns[0]));

                    }

                    if (ds.Tables.Count == 3)
                    {
                        ds.Tables[0].TableName = "ParentTabled";
                        ds.Tables[1].TableName = "ChildTabled";
                        ds.Tables[2].TableName = "ChildTabled3";
                        //// Set the table's primary key field
                        ds.Tables["ParentTabled"].PrimaryKey = new DataColumn[] { ds.Tables["ParentTabled"].Columns[0] };
                        ds.AcceptChanges();
                        ds.Relations.Add(new DataRelation("relacion", ds.Tables[0].Columns[0], ds.Tables[1].Columns[0]));


                        ds.Tables["ChildTabled"].PrimaryKey = new DataColumn[] { ds.Tables["ChildTabled"].Columns[1] };
                        ds.AcceptChanges();
                        ds.Relations.Add(new DataRelation("relacion2", ds.Tables[1].Columns[1], ds.Tables[2].Columns[1]));

                    }
                }
            }
            catch (Exception e)
            {
                throw new System.InvalidOperationException("Error de datos:" + e.Message);

            }
            finally
            {
                cnn.Close();
                //cnn.Dispose();
            }
            return ds;
        }
        public virtual DataSet funGetbyStoresPropios(string pNombreStored, string[,] pArreglo)
        {
            //'Creación de la función pública utilizada para llamar a todos los storeds
            //       ' pasando como parámetros el nombre del stored correspondiente y un 
            //       ' arreglo(nombre de la variable, tipo de dato, valor de la varible)
            cnn.Open();

            DataSet ds = new DataSet();
            try
            {

                if (pArreglo.GetUpperBound(1) > 0)
                {
                    int upper = pArreglo.GetUpperBound(1);
                    int lower = pArreglo.GetLowerBound(0);

                    SqlParameter[] sqlParams = new SqlParameter[pArreglo.GetUpperBound(0) + 1];
                    for (int ctr = pArreglo.GetLowerBound(0); ctr <= pArreglo.GetUpperBound(0); ++ctr)
                    {
                        sqlParams[ctr] = new SqlParameter(pArreglo[ctr, 0].ToString(), TipoDato(pArreglo[ctr, 1]));
                        sqlParams[ctr].Value = pArreglo[ctr, 2];
                    }

                    //'Cuando se pasan parametros
                    ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored, sqlParams));
                }
                else
                {
                    //'Para storeds que no necesitan pase de parametros
                    ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored));
                }

                ds.AcceptChanges();

            }
            catch (Exception e)
            {
                throw new System.InvalidOperationException("Error de datos:" + e.Message);

            }
            finally
            {
                cnn.Close();
                //cnn.Dispose();
            }
            return ds;
        }
        public virtual SqlDbType TipoDato(string pNumTipo)
        {
            switch (pNumTipo)
            {
                case "0":
                    return SqlDbType.BigInt;
                    break;

                case "1":
                    return SqlDbType.Binary;
                    break;
                case "2":
                    return SqlDbType.Bit;
                    break;
                case "3":
                    return SqlDbType.Char;
                    break;
                case "4":
                    return SqlDbType.DateTime;
                    break;
                case "5":
                    return SqlDbType.Decimal;
                    break;
                case "6":
                    return SqlDbType.Float;
                    break;
                case "7":
                    return SqlDbType.Image;
                    break;
                case "8":
                    return SqlDbType.Int;
                    break;
                case "9":
                    return SqlDbType.Money;
                    break;
                case "10":
                    return SqlDbType.NChar;
                    break;
                case "11":
                    return SqlDbType.NText;
                    break;
                case "12":
                    return SqlDbType.NVarChar;
                    break;
                case "13":
                    return SqlDbType.Real;
                    break;
                case "14":
                    return SqlDbType.SmallDateTime;
                    break;
                case "15":
                    return SqlDbType.SmallInt;
                    break;
                case "16":
                    return SqlDbType.SmallMoney;
                    break;
                case "17":
                    return SqlDbType.Text;
                    break;
                case "18":
                    return SqlDbType.Timestamp;
                    break;
                case "19":
                    return SqlDbType.TinyInt;
                    break;
                case "20":
                    return SqlDbType.UniqueIdentifier;
                    break;
                case "21":
                    return SqlDbType.VarBinary;
                    break;
                case "22":
                    return SqlDbType.VarChar;
                    break;
                case "23":
                    return SqlDbType.Variant;
                    break;

                default:
                    return System.Data.SqlDbType.NVarChar;
                    break;
            }

        }
        #endregion
        //------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------COMITE-----------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------
    }
}