using System;
using Encuestasopm.Datos;

namespace Encuestasopm.Admin
{
    public partial class Panel : System.Web.UI.Page
    {
        protected string PlanesJson = "[]";

        protected void Page_Load(object sender, EventArgs e)
        {
            try { PlanesJson = Json.Serializar(EncuestaDatos.Catalogos()["planes"]); }
            catch (Exception ex) { Bitacora.Error("Panel catálogos", ex); }
        }
    }
}
