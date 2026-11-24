using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;//
using System.Data.SqlClient;//
using System.Collections;
using System.Configuration;
using System.Web.Script.Services;
using Newtonsoft.Json;
using Encuestasopm.Modelos;
using System.ComponentModel;
using System.IO;

namespace Encuestasopm
{
    /// <summary>
    /// Descripción breve de WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
     [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }
        [WebMethod]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public JsonReader EnviarDenuncia(string data)
        //{
        public string EnviarDenuncia(String anonimo, String notificar, String txtnombre, String txtdireccion, String txttelefono, String txtemail,
      String sltentidadocurre, String txtfechainiciocurrio, String txtfechafinocurrio, String txtnarracion, String txtnomdenunciado, String txtpatdenunciado,
      String txtmatdenunciado, String sltDependencia, String txtadscripcion, String txtpuesto, String sltentidaddenunciado, String txtnombreconsta,
      String txtdireccionconsta, String txttelefonoconsta, String txtemailconsta, String radioservidor, String sltDependenciaConsta, String txtcargoconsta)
        {

            String resultado = "";
            String respBD;
            combos BDEtica = new combos();
            resultado respuesta = new resultado();
            List<string> result = new List<string>();

            try
            {
                respBD = BDEtica.EnviarDenunciaBD(anonimo, notificar, txtnombre, txtdireccion, txttelefono, txtemail, sltentidadocurre, txtfechainiciocurrio,
                        txtfechafinocurrio, txtnarracion, txtnomdenunciado, txtpatdenunciado, txtmatdenunciado, sltDependencia, txtadscripcion,
                        txtpuesto, sltentidaddenunciado, txtnombreconsta, txtdireccionconsta, txttelefonoconsta, txtemailconsta, radioservidor,
                        sltDependenciaConsta, txtcargoconsta);
                switch (respBD)
                {
                    case "EI":
                        respuesta.respuesta = "EI";
                        result.Add(string.Format("{0}", "EI"));
                        resultado = "EI";
                        break;
                    case "EEX":
                        respuesta.respuesta = "EEX";
                        result.Add(string.Format("{0}", "EEX"));
                        resultado = "EEX";
                        break;
                    case "UEX":
                        respuesta.respuesta = "UEX";
                        result.Add(string.Format("{0}", "UEX"));
                        resultado = "UEX";
                        break;
                    default:
                        respuesta.respuesta = "OK";
                        result.Add(string.Format("{0}", "OK"));
                        resultado = "OK";
                        //EnviarCorreoElectronicoReg(respBD, txtnombre, txttelefono, txtemail, "00007-20");
                        break;
                }
            }
            catch (Exception ex)
            {
                resultado = "Ocurrió un error en el servidor. Inténtelo de nuevo." + ex;
            }
            return "conectado";
        }
        [WebMethod]
        public Boolean Hola()
        {
            return true;
        }

        [WebMethod]
        public List<quejaDetalleClass> VerDetalleQueja(String idqueja)
        {
            List<quejaDetalleClass> lstquejaDetalle = new List<quejaDetalleClass>();
            combos BDEtica = new combos();
            DataTable dt = new DataTable();
            try
            {
                dt = BDEtica.quejaDetalleBD(idqueja);
                quejaDetalleClass quejaDetalle;
                foreach (DataRow fila in dt.Rows)
                {
                    quejaDetalle = new quejaDetalleClass();
                    quejaDetalle.idqueja = fila["IDqueja"].ToString();
                    quejaDetalle.folioqueja = "";
                    quejaDetalle.tipo = fila["Anomino"].ToString();
                    quejaDetalle.notificar = fila["PersonaNotificar"].ToString();
                    quejaDetalle.nomvic = fila["Nombre"].ToString();
                    quejaDetalle.direccionvic = fila["Direccion"].ToString();
                    quejaDetalle.telefonovic = fila["Telefono"].ToString();
                    quejaDetalle.emailvic = fila["Email"].ToString();
                    quejaDetalle.entidadhecho = fila["Entidafederativaocurrio"].ToString();
                    quejaDetalle.fechaini = fila["Fechainicioocurrio"].ToString();
                    quejaDetalle.fechafin = fila["Fechafinocurrio"].ToString();
                    quejaDetalle.narracion = fila["narracionhecho"].ToString();
                    quejaDetalle.nombrecontra = fila["Nombrecontra"].ToString();
                    quejaDetalle.depcontra = fila["Dependencia"].ToString();
                    quejaDetalle.adscontra = fila["Areaadscripcion"].ToString();
                    quejaDetalle.cargocontra = fila["Cargopuesto"].ToString();
                    quejaDetalle.entidadcontra = fila["Entidafederativa"].ToString();
                    quejaDetalle.nomconsta = fila["Nombreconsta"].ToString();
                    quejaDetalle.dirconsta = fila["Direccionconsta"].ToString();
                    quejaDetalle.telconsta = fila["Telefonoconsta"].ToString();
                    quejaDetalle.emailconsta = fila["Emailconsta"].ToString();
                    quejaDetalle.publico = fila["Trabajadorpublico"].ToString();
                    quejaDetalle.depenconsta = fila["Dependenciatrabajador"].ToString();
                    quejaDetalle.cargoconsta = fila["Cargotrabajador"].ToString();
                    quejaDetalle.fechareg = fila["Fechaalta"].ToString();
                    quejaDetalle.estatus = fila["Estatus"].ToString();
                    lstquejaDetalle.Add(quejaDetalle);
                }
            }
            catch (Exception ex)
            {

            }
            return lstquejaDetalle;
            //return Json(lstquejaDetalle, JsonRequestBehavior.AllowGet);
        }
        //====================================================================================================================================================================
        //==========================================ENCUESTA OCOSINGO=========================================================================================================
        //====================================================================================================================================================================
        [WebMethod]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public JsonReader EnviarDenuncia(string data)
        //{
        public string encuestaocosingo( String txtnomciudadano, String cbogenero, String radioservidoredad,String cboocupacion,String cbonacido, String idplanes,String cboexistencia,String cboapoyo,String txtproblematica,String txtpropuesta,String inseguridad, String bienestar,String abandono,String economico,String desatencion,String archivo, String cboconfianza)
        {
            String resultado = "";
            String respBD;
            combos BDEtica = new combos();
            resultado respuesta = new resultado();
            List<string> result = new List<string>();
            try
            {
                respBD = BDEtica.EnviarencuestaBD(txtnomciudadano, cbogenero, radioservidoredad, cboocupacion, cbonacido, idplanes, cboexistencia,cboapoyo, txtproblematica, txtpropuesta, inseguridad, bienestar, abandono, economico, desatencion, archivo, cboconfianza);
                
            }
            catch (Exception ex)
            {
                resultado = "Ocurrió un error en el servidor. Inténtelo de nuevo." + ex;
            }

            return "conectado";
        }


        [WebMethod]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public JsonReader EnviarDenuncia(string data)
        //{
        public string prueba(String anonimo, String notificar, String txtnombre, String txtdireccion,
          String sltentidadocurre, String txtfechainiciocurrio, String txtfechafinocurrio, String txtnomdenunciado, String txtpatdenunciado,
          String txtmatdenunciado, String txtnombreconsta,
          String txtdireccionconsta)
        {

            String resultado = "";
            String respBD;
            combos BDEtica = new combos();
            resultado respuesta = new resultado();
            List<string> result = new List<string>();


            return "conectado";
        }

        [WebMethod]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public JsonReader EnviarDenuncia(string data)
        //{
        public string archivo(HttpPostedFileBase file)
        {

            string path = Server.MapPath("~/uploads/");
            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);
                file.SaveAs(path + fileName);
            }


            return "Success";
        }
        [WebMethod]

        public String UploadFile()
        {
            string BaseDirectory = "";
            string result = "";
            int bookid = 0;
            string nameBookFile = "";
            string nameSmallCoverFile = "";
            string nameRegularCoverFile = "";
            string title_crypto = "";
            string fileBase64 = "";
            bool RowsFound = false;


            HttpContext context = HttpContext.Current;

            try
            {
                //Capture single file From Post
                //HttpPostedFile file = context.Request.Files[0];
                //name = file.FileName;

                //Capture multiple files From Post                
                for (int i = 0; i < context.Request.Files.Count; ++i)
                {
                    HttpPostedFile file = context.Request.Files[i];
                    //if (file.FileName.IndexOf("rmov") != -1)
                    //{
                    //    nameBookFile = file.FileName;
                    //    BaseDirectory = Server.MapPath("~/books/");
                    //}
                    //if (file.FileName.IndexOf("small") != -1)
                    //{
                    //    nameSmallCoverFile = file.FileName;
                    //    BaseDirectory = Server.MapPath("~/covers/");
                    //}
                    //if (file.FileName.IndexOf("regular") != -1)
                    //{
                    //    nameRegularCoverFile = file.FileName;
                    //    BaseDirectory = Server.MapPath("~/covers/");
                    //}


                    //Optional: Convert to File to binary if you need to forward it to a video encoding service like Panda Stream
                    /*BinaryReader b = new BinaryReader(file.InputStream);
                      byte[] binData = b.ReadBytes(file.ContentLength);
                      b.Close();
                      Base64Process file_info = new Base64Process();
                      fileBase64 = file_info.Encode64(binData);*/

                    //create dir if doesn't exist
                    //if (!checkfolderexists(BaseDirectory))
                    //{
                    //    throw new Exception("Base Folder does not exist and could not create.");
                    //}
                    BaseDirectory = Server.MapPath("~/uploads/");
                    //Or just save it locally
                    file.SaveAs(BaseDirectory + '/' + file.FileName);

                }



                result = result + "File saved successfully as file ";

            }
            catch (Exception exc)
            {
                result = "Error: " + exc.Message;
            }

            return result;
        }
        //====================================================================================================================================================================
        //==========================================ENCUESTA OCOSINGO=========================================================================================================
        //====================================================================================================================================================================
    }
}
