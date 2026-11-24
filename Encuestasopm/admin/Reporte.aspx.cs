using System;
using System.Collections.Generic;
using System.Web;
using Encuestasopm.Datos;

namespace Encuestasopm.Admin
{
    public partial class Reporte : System.Web.UI.Page
    {
        private const int MaxAnexo = 150;

        protected string DatosJson = "null";
        protected string Periodo = "Todo el periodo";
        protected string Alcance = "Todos los planes y programas";
        protected string Elaboro = "";
        protected bool HayError;

        protected void Page_Load(object sender, EventArgs e)
        {
            var u = Seguridad.UsuarioActual();
            Elaboro = HttpUtility.HtmlEncode(u == null ? "" : u.Nombre);

            var f = FiltroPanel.DesdeRequest(Request);
            f.Buscar = null;
            if (f.Desde.HasValue || f.Hasta.HasValue)
                Periodo = (f.Desde.HasValue ? f.Desde.Value.ToString("dd/MM/yyyy") : "inicio") + " al " +
                          (f.Hasta.HasValue ? f.Hasta.Value.ToString("dd/MM/yyyy") : DateTime.Today.ToString("dd/MM/yyyy"));
            if (f.IdTipo == 1) Alcance = "Solo planes";
            if (f.IdTipo == 2) Alcance = "Solo programas";

            try
            {
                var resumen = EncuestaDatos.Resumen(f);
                if (f.IdPlanes.HasValue)
                {
                    foreach (var p in (List<Dictionary<string, object>>)resumen["planes"])
                        if (Convert.ToInt32(p["idplanes"]) == f.IdPlanes.Value) Alcance = HttpUtility.HtmlEncode(Convert.ToString(p["nombre"]));
                }
                resumen["anexo"] = Db.ALista(EncuestaDatos.Listado(f, MaxAnexo));
                resumen["maxAnexo"] = MaxAnexo;
                DatosJson = Json.Serializar(resumen);
            }
            catch (Exception ex)
            {
                Bitacora.Error("Reporte", ex);
                HayError = true;
            }
        }
    }
}
