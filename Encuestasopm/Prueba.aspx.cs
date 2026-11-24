using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Encuesta;

namespace Encuestasopm
{
    public partial class Prueba : System.Web.UI.Page
    {
        Cnn cn;
        ArrayList arrayParamValue = new ArrayList();
        String Nombre;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
               
            }
        }

        protected void planes_ServerClick(object sender, EventArgs e)
        {

            //Int64 idanio = Convert.ToInt32(cboanio.SelectedValue);

            Nombre = "Reportes\\Rptplanes.rpt";


           // Session["xIDAnio"] = idanio.ToString();
            Session["xNombre"] = Nombre;
            Session["xNreporte"] = "1";


            hidden_link.Attributes["href"] = "ReportesPlanes.aspx";
            string someScript = "";
            someScript = "<script type='text/javascript'>jQuery(document).ready(function() {miFuncion();});</script>";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Aceptar", someScript, false);
        }

        protected void temas_ServerClick(object sender, EventArgs e)
        {

        }

        protected void confianza_ServerClick(object sender, EventArgs e)
        {

        }
    }
}