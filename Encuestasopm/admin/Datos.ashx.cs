using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using Encuestasopm.Datos;

namespace Encuestasopm.Admin
{
    /// <summary>
    /// Servicio del panel (protegido por el login, ver Web.config &lt;location path="admin"&gt;).
    ///   ?accion=resumen   -> indicadores y series para las gráficas (JSON)
    ///   ?accion=listado   -> últimas 300 encuestas con filtros y búsqueda (JSON)
    ///   ?accion=csv       -> todas las encuestas filtradas en CSV para Excel
    ///   ?accion=adjunto&amp;id=N -> descarga el archivo que adjuntó el ciudadano
    ///   POST accion=password -> cambio de contraseña del usuario actual
    /// </summary>
    public class DatosHandler : IHttpHandler
    {
        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext ctx)
        {
            var usuario = Seguridad.UsuarioActual();
            if (usuario == null) { ctx.Response.StatusCode = 401; return; }
            ctx.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            try
            {
                var filtro = FiltroPanel.DesdeRequest(ctx.Request);
                switch (ctx.Request["accion"])
                {
                    case "resumen":
                        Json.Escribir(ctx, EncuestaDatos.Resumen(filtro));
                        break;
                    case "listado":
                        Json.Escribir(ctx, Db.ALista(EncuestaDatos.Listado(filtro, 300)));
                        break;
                    case "csv":
                        Csv(ctx, EncuestaDatos.Listado(filtro, null));
                        break;
                    case "adjunto":
                        Adjunto(ctx);
                        break;
                    case "password":
                        CambiarPassword(ctx, usuario);
                        break;
                    default:
                        ctx.Response.StatusCode = 400;
                        break;
                }
            }
            catch (Exception ex)
            {
                Bitacora.Error("Panel " + ctx.Request["accion"], ex);
                ctx.Response.Clear();
                ctx.Response.StatusCode = 500;
                Json.Escribir(ctx, new { ok = false, mensaje = "Ocurrió un error al consultar la información." });
            }
        }

        private static void CambiarPassword(HttpContext ctx, UsuarioPanel u)
        {
            // Solo POST desde la propia página (el encabezado personalizado no se puede enviar desde otro sitio).
            if (ctx.Request.HttpMethod != "POST" || ctx.Request.Headers["X-Requested-With"] != "fetch")
            {
                ctx.Response.StatusCode = 400;
                return;
            }
            string actual = ctx.Request.Form["actual"], nueva = ctx.Request.Form["nueva"];
            if (Seguridad.Validar(u.Usuario, actual) == null)
            {
                Json.Escribir(ctx, new { ok = false, mensaje = "La contraseña actual no es correcta." });
                return;
            }
            if (string.IsNullOrEmpty(nueva) || nueva.Length < 10)
            {
                Json.Escribir(ctx, new { ok = false, mensaje = "La nueva contraseña debe tener al menos 10 caracteres." });
                return;
            }
            Seguridad.CambiarPassword(u.Id, nueva);
            Json.Escribir(ctx, new { ok = true, mensaje = "Contraseña actualizada." });
        }

        private static void Adjunto(HttpContext ctx)
        {
            int id;
            string[] adj = int.TryParse(ctx.Request["id"], out id) ? EncuestaDatos.Adjunto(id) : null;
            string ruta = adj == null ? null : Path.Combine(ctx.Server.MapPath("~/App_Data/adjuntos"), Path.GetFileName(adj[0]));
            if (ruta == null || !File.Exists(ruta))
            {
                ctx.Response.StatusCode = 404;
                ctx.Response.ContentType = "text/plain";
                ctx.Response.Write("El archivo no existe.");
                return;
            }
            string nombre = string.IsNullOrEmpty(adj[1]) ? adj[0] : adj[1];
            ctx.Response.ContentType = MimeMapping.GetMimeMapping(ruta);
            ctx.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Uri.EscapeDataString(nombre) + "\"; filename*=UTF-8''" + Uri.EscapeDataString(nombre));
            ctx.Response.AddHeader("X-Content-Type-Options", "nosniff");
            ctx.Response.TransmitFile(ruta);
        }

        private static void Csv(HttpContext ctx, DataTable dt)
        {
            string[] columnas = { "folio", "fechaencuesta", "tipo", "planprograma", "nombre", "genero", "edad", "ocupacion",
                                  "residente", "zona", "localidad", "conoce", "apoyo", "temas", "problematica", "propuesta",
                                  "archivooriginal", "confianza" };
            string[] titulos = { "Folio", "Fecha", "Tipo", "Plan o programa", "Nombre", "Género", "Edad", "Ocupación",
                                 "Vive en Ocosingo", "Zona", "Localidad", "Conocía el plan/programa", "Ha recibido apoyo",
                                 "Temas prioritarios", "Problemática", "Propuesta", "Archivo adjunto", "Confianza en el gobierno" };

            var sb = new StringBuilder();
            sb.AppendLine(string.Join(",", Array.ConvertAll(titulos, Celda)));
            foreach (DataRow r in dt.Rows)
            {
                var valores = new string[columnas.Length];
                for (int i = 0; i < columnas.Length; i++)
                {
                    object v = r[columnas[i]];
                    valores[i] = Celda(v == DBNull.Value ? "" : v is DateTime ? ((DateTime)v).ToString("yyyy-MM-dd HH:mm") : Convert.ToString(v));
                }
                sb.AppendLine(string.Join(",", valores));
            }

            ctx.Response.ContentType = "text/csv";
            ctx.Response.ContentEncoding = new UTF8Encoding(false);
            ctx.Response.AddHeader("Content-Disposition", "attachment; filename=encuestas_ocosingo_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv");
            ctx.Response.BinaryWrite(new UTF8Encoding(true).GetPreamble());   // BOM para que Excel respete los acentos
            ctx.Response.Write(sb.ToString());
        }

        private static string Celda(string s)
        {
            s = (s ?? "").Replace("\r", " ").Replace("\n", " ");
            // Evita "inyección de fórmulas" al abrir el CSV en Excel.
            if (s.Length > 0 && "=+-@".IndexOf(s[0]) >= 0) s = "'" + s;
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        }
    }
}
