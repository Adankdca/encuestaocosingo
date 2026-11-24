using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Encuestasopm.Datos;

namespace Encuestasopm
{
    /// <summary>
    /// Recibe la encuesta completa (datos + archivo opcional) en un solo POST multipart/form-data.
    /// Valida todo en el servidor, guarda el adjunto con nombre aleatorio fuera del sitio público
    /// (~/App_Data/adjuntos) y responde JSON: { ok } o { ok:false, mensaje }.
    /// El folio se guarda en la base de datos para uso interno (panel y reporte); no se muestra al ciudadano.
    /// </summary>
    public class EncuestaHandler : IHttpHandler
    {
        public bool IsReusable { get { return true; } }

        private const int MaxBytesArchivo = 5 * 1024 * 1024;
        private static readonly string[] Extensiones = { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx" };

        public void ProcessRequest(HttpContext ctx)
        {
            ctx.Response.ContentType = "application/json";
            ctx.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (ctx.Request.HttpMethod != "POST")
            {
                ctx.Response.StatusCode = 405;
                Json.Escribir(ctx, new { ok = false, mensaje = "Método no permitido." });
                return;
            }

            var f = ctx.Request.Form;

            // Trampa para robots: el campo "sitioweb" está oculto para las personas.
            if (!string.IsNullOrEmpty(f["sitioweb"]))
            {
                Json.Escribir(ctx, new { ok = true });
                return;
            }

            if (!Seguridad.PermitirIntento("encuesta", 5, TimeSpan.FromMinutes(10)))
            {
                ctx.Response.StatusCode = 429;
                Json.Escribir(ctx, new { ok = false, mensaje = "Recibimos varias encuestas desde tu conexión. Intenta de nuevo en unos minutos." });
                return;
            }

            var errores = new List<string>();
            var cat = EncuestaDatos.Catalogos();

            var e = new EncuestaNueva
            {
                Nombre = Texto(f["nombre"], 150),
                Genero = Opcion(f["genero"], new[] { "M", "F", "O" }, "Género", errores),
                IdEdad = IdCatalogo(f["edad"], cat["edades"], "id", "Rango de edad", errores),
                IdOcupacion = IdCatalogo(f["ocupacion"], cat["ocupaciones"], "id", "Ocupación", errores),
                Residente = Opcion(f["residente"], new[] { "SI", "NO" }, "¿Vives en Ocosingo?", errores),
                Zona = string.IsNullOrEmpty(f["zona"]) ? null : Opcion(f["zona"], new[] { "C", "R" }, "Zona", errores),
                Localidad = Texto(f["localidad"], 150),
                IdPlanes = IdCatalogo(f["plan"], cat["planes"], "idplanes", "Plan o programa", errores),
                Existencia = Opcion(f["conoce"], new[] { "SI", "NO" }, "¿Conocías el plan o programa?", errores),
                Apoyo = Opcion(f["apoyo"], new[] { "SI", "NO" }, "¿Has recibido apoyo?", errores),
                Inseguridad = Marcado(f, "inseguridad"),
                Bienestar = Marcado(f, "bienestar"),
                Abandono = Marcado(f, "abandono"),
                Economico = Marcado(f, "economico"),
                Desatencion = Marcado(f, "desatencion"),
                Servicios = Marcado(f, "servicios"),
                Problematica = Texto(f["problematica"], 2000),
                Propuesta = Texto(f["propuesta"], 2000),
                IdConfianza = IdCatalogo(f["confianza"], cat["confianza"], "id", "Confianza en el gobierno", errores)
            };

            if (e.Residente != "SI") { e.Zona = null; e.Localidad = null; }
            if (!(e.Inseguridad || e.Bienestar || e.Abandono || e.Economico || e.Desatencion || e.Servicios))
                errores.Add("Selecciona al menos un tema prioritario.");
            if (string.IsNullOrEmpty(e.Problematica) || e.Problematica.Length < 10)
                errores.Add("Describe la problemática (mínimo 10 caracteres).");
            if (f["aviso"] != "1")
                errores.Add("Debes aceptar el aviso de privacidad.");

            HttpPostedFile archivo = ctx.Request.Files.Count > 0 ? ctx.Request.Files["archivo"] : null;
            if (archivo != null && archivo.ContentLength > 0)
            {
                string error = ValidarArchivo(archivo);
                if (error != null) errores.Add(error);
            }
            else archivo = null;

            if (errores.Count > 0)
            {
                ctx.Response.StatusCode = 400;
                Json.Escribir(ctx, new { ok = false, mensaje = string.Join(" ", errores) });
                return;
            }

            string rutaGuardada = null;
            try
            {
                if (archivo != null)
                {
                    string ext = Path.GetExtension(archivo.FileName).ToLowerInvariant();
                    string carpeta = ctx.Server.MapPath("~/App_Data/adjuntos");
                    Directory.CreateDirectory(carpeta);
                    e.Archivo = DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString("N") + ext;
                    e.ArchivoOriginal = Texto(Path.GetFileName(archivo.FileName), 200);
                    rutaGuardada = Path.Combine(carpeta, e.Archivo);
                    archivo.SaveAs(rutaGuardada);
                }

                EncuestaDatos.Guardar(e);
                Json.Escribir(ctx, new { ok = true });
            }
            catch (Exception ex)
            {
                if (rutaGuardada != null && File.Exists(rutaGuardada)) File.Delete(rutaGuardada);
                Bitacora.Error("Guardar encuesta", ex);
                ctx.Response.StatusCode = 500;
                Json.Escribir(ctx, new { ok = false, mensaje = "No pudimos guardar tu encuesta en este momento. Intenta de nuevo más tarde." });
            }
        }

        private static string ValidarArchivo(HttpPostedFile a)
        {
            string ext = Path.GetExtension(a.FileName ?? "").ToLowerInvariant();
            if (!Extensiones.Contains(ext))
                return "El archivo debe ser PDF, imagen (JPG o PNG), Word o Excel.";
            if (a.ContentLength > MaxBytesArchivo)
                return "El archivo no debe pesar más de 5 MB.";

            // Revisa la "firma" real del archivo para que no se pueda disfrazar otro tipo con la extensión.
            var cab = new byte[8];
            var s = a.InputStream;
            s.Position = 0;
            int leidos = s.Read(cab, 0, cab.Length);
            s.Position = 0;
            if (leidos < 4) return "El archivo está vacío o dañado.";

            bool valido;
            switch (ext)
            {
                case ".pdf": valido = cab[0] == 0x25 && cab[1] == 0x50 && cab[2] == 0x44 && cab[3] == 0x46; break;           // %PDF
                case ".png": valido = cab[0] == 0x89 && cab[1] == 0x50 && cab[2] == 0x4E && cab[3] == 0x47; break;           // .PNG
                case ".jpg":
                case ".jpeg": valido = cab[0] == 0xFF && cab[1] == 0xD8 && cab[2] == 0xFF; break;
                case ".docx":
                case ".xlsx": valido = cab[0] == 0x50 && cab[1] == 0x4B && cab[2] == 0x03 && cab[3] == 0x04; break;          // ZIP (Office moderno)
                default: valido = cab[0] == 0xD0 && cab[1] == 0xCF && cab[2] == 0x11 && cab[3] == 0xE0; break;                // .doc/.xls (OLE)
            }
            return valido ? null : "El contenido del archivo no corresponde a su extensión.";
        }

        private static string Texto(string valor, int max)
        {
            if (string.IsNullOrWhiteSpace(valor)) return null;
            valor = valor.Trim();
            return valor.Length > max ? valor.Substring(0, max) : valor;
        }

        private static bool Marcado(System.Collections.Specialized.NameValueCollection f, string tema)
        {
            var temas = f.GetValues("temas");
            return temas != null && temas.Contains(tema);
        }

        private static string Opcion(string valor, string[] permitidos, string campo, List<string> errores)
        {
            if (valor != null && permitidos.Contains(valor)) return valor;
            errores.Add("Responde: " + campo + ".");
            return null;
        }

        private static int IdCatalogo(string valor, object catalogo, string llave, string campo, List<string> errores)
        {
            int id;
            if (int.TryParse(valor, out id))
            {
                foreach (var fila in (List<Dictionary<string, object>>)catalogo)
                    if (Convert.ToInt32(fila[llave]) == id) return id;
            }
            errores.Add("Responde: " + campo + ".");
            return 0;
        }
    }
}
