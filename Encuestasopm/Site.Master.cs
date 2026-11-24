using System;
using System.Configuration;
using System.Web;

namespace Encuestasopm
{
    public partial class Site : System.Web.UI.MasterPage
    {
        /// <summary>Resuelve "~/..." y agrega ?v= a CSS/JS para invalidar la caché del navegador al publicar.</summary>
        public string Url(string ruta)
        {
            return Recursos.Url(ResolveUrl(ruta));
        }
    }

    public static class Recursos
    {
        private static readonly string Version = ConfigurationManager.AppSettings["VersionRecursos"] ?? "1";

        public static string Url(string resuelta)
        {
            if (resuelta.EndsWith(".css") || resuelta.EndsWith(".js")) resuelta += "?v=" + Version;
            return HttpUtility.HtmlAttributeEncode(resuelta);
        }
    }
}
