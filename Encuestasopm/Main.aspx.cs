using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Encuestasopm.Controladores;

namespace Encuestasopm
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Controlador datos = new Controlador();

            if (Request.QueryString["tipo"] == "0")
                Response.Write(datos.getwizard());
            if (Request.QueryString["tipo"] == "1")
                Response.Write(datos.preguntas(Request.QueryString["idplan"]));
        }
    }
}