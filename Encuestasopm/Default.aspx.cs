using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Encuestasopm.Datos;

namespace Encuestasopm
{
    public partial class Default : System.Web.UI.Page
    {
        protected string CatalogosJson = "null";
        protected bool EncuestaDisponible;
        private Dictionary<string, object> catalogos;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                catalogos = EncuestaDatos.Catalogos();
                CatalogosJson = Json.Serializar(catalogos);
                EncuestaDisponible = true;
            }
            catch (Exception ex)
            {
                // Si la base de datos no responde la página se muestra igual, con la encuesta deshabilitada.
                Bitacora.Error("Cargar catálogos", ex);
                EncuestaDisponible = false;
            }
        }

        /// <summary>Tarjetas de la sección "Planes y programas" (1 = planes, 2 = programas).</summary>
        protected string TarjetasHtml(int tipo)
        {
            if (catalogos == null) return "";
            var sb = new StringBuilder();
            foreach (var p in (List<Dictionary<string, object>>)catalogos["planes"])
            {
                if (Convert.ToInt32(p["idtipo"]) != tipo) continue;
                string icono = HttpUtility.HtmlAttributeEncode("img/" + Convert.ToString(p["icono"]));
                sb.Append("<div class=\"col-md-6 col-lg-12\"><article class=\"tarjeta-plan\">")
                  .Append("<img src=\"").Append(icono).Append("\" alt=\"\" width=\"56\" height=\"56\" loading=\"lazy\" />")
                  .Append("<div><h3>").Append(HttpUtility.HtmlEncode(p["nombre"])).Append("</h3>")
                  .Append("<p>").Append(HttpUtility.HtmlEncode(p["descripcion"])).Append("</p>")
                  .Append("<button type=\"button\" class=\"btn btn-contorno\" data-abrir-encuesta data-plan=\"")
                  .Append(Convert.ToInt32(p["idplanes"])).Append("\">Opinar sobre este</button>")
                  .Append("</div></article></div>");
            }
            return sb.ToString();
        }
    }
}
