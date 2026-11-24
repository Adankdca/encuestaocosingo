using System;
using System.IO;
using System.Web;
using Encuestasopm.Datos;

namespace Encuestasopm
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected string NombreUsuario
        {
            get
            {
                var u = Seguridad.UsuarioActual();
                return HttpUtility.HtmlEncode(u == null ? "Usuario" : u.Nombre);
            }
        }

        public string Url(string ruta)
        {
            return Recursos.Url(ResolveUrl(ruta));
        }

        protected string Activo(string pagina)
        {
            return Path.GetFileNameWithoutExtension(Request.Path).Equals(pagina, StringComparison.OrdinalIgnoreCase) ? "activo" : "";
        }
    }
}
