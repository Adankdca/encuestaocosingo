using System;
using System.Web;
using System.Web.Security;
using Encuestasopm.Datos;

namespace Encuestasopm
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["salir"] == "1")
            {
                FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            if (!IsPostBack && Seguridad.UsuarioActual() != null)
            {
                Response.Redirect(FormsAuthentication.DefaultUrl, false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            if (!Seguridad.PermitirIntento("login", 10, TimeSpan.FromMinutes(15)))
            {
                Mostrar("Demasiados intentos. Espera 15 minutos e inténtalo de nuevo.");
                return;
            }

            UsuarioPanel u;
            try
            {
                u = Seguridad.Validar(txtUsuario.Text, txtPassword.Text);
            }
            catch (Exception ex)
            {
                Bitacora.Error("Login", ex);
                Mostrar("No hay conexión con la base de datos. Avisa al administrador del sistema.");
                return;
            }

            if (u == null)
            {
                Mostrar("Usuario o contraseña incorrectos.");
                return;
            }

            Seguridad.IniciarSesion(u);
            // GetRedirectUrl solo devuelve direcciones del propio sitio (evita redirecciones abiertas).
            Response.Redirect(FormsAuthentication.GetRedirectUrl(u.Usuario, false), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void Mostrar(string mensaje)
        {
            litMensaje.Text = HttpUtility.HtmlEncode(mensaje);
            pnlMensaje.Visible = true;
        }
    }
}
