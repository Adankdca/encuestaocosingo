using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;

namespace Encuestasopm.Datos
{
    public static class Json
    {
        public static string Serializar(object o)
        {
            // JavaScriptSerializer escapa <, > y & como <, por lo que es seguro incrustarlo en <script>.
            var js = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            return js.Serialize(o);
        }

        public static void Escribir(HttpContext ctx, object o)
        {
            ctx.Response.ContentType = "application/json";
            ctx.Response.ContentEncoding = System.Text.Encoding.UTF8;
            ctx.Response.Write(Serializar(o));
        }
    }

    /// <summary>Registro de errores en ~/App_Data/logs (una carpeta que IIS no publica).</summary>
    public static class Bitacora
    {
        private static readonly object Candado = new object();

        public static void Error(string contexto, Exception ex)
        {
            try
            {
                string carpeta = HostingEnvironment.MapPath("~/App_Data/logs");
                Directory.CreateDirectory(carpeta);
                string archivo = Path.Combine(carpeta, "errores_" + DateTime.Now.ToString("yyyyMM") + ".log");
                string linea = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + contexto + " | " + ex + Environment.NewLine;
                lock (Candado) File.AppendAllText(archivo, linea);
            }
            catch
            {
                // Nunca dejamos que el registro de errores tire la petición.
            }
        }
    }
}
