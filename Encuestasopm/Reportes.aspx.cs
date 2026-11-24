using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Encuestasopm
{
    public partial class Reportes : System.Web.UI.Page
    {
        Encuesta.Cnn cn;
        ArrayList arrayParamValue = new ArrayList();
        String Nombre;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                Page.MaintainScrollPositionOnPostBack = true;
                //string someScript1 = "";
                //someScript1 = "<script type='text/javascript'>inicial();</script>";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Aceptar", someScript1, false);
            }

        }

  

        protected void reporte1_ServerClick(object sender, EventArgs e)
        {
            Nombre = "Reportes\\Rptplanes.rpt";

            // Session["xIDAnio"] = idanio.ToString();
            Session["xNombre"] = Nombre;
             Session["xNreporte"] = "1";
            //Response.Redirect("ReportesPlanes.aspx");
            abreVentana("ReportesPlanes.aspx", "_parent");
            //hidden_link.Attributes["href"] = "ReportesPlanes.aspx";
            //string someScript = "";
            //someScript = "<script type='text/javascript'>jQuery(document).ready(function() {miFuncion();});</script>";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "Aceptar", someScript, false);

        }
        private void abreVentana(string ventana, string target)
        {
            string Clientscript = "<script>window.open('" +
                               ventana + "','" + target +
                               "')</script>";

            if (!Page.ClientScript.IsStartupScriptRegistered("WOpen"))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "WOpen", Clientscript);
            }
        }

        protected void reporte2_temas_ServerClick(object sender, EventArgs e)
        {
            Nombre = "Reportes\\Rpttemas.rpt";

            // Session["xIDAnio"] = idanio.ToString();
            Session["xNombre"] = Nombre;
            Session["xNreporte"] = "2";
            //Response.Redirect("ReportesPlanes.aspx");
            abreVentana("ReportesPlanes.aspx", "_parent");
        }
    }
}