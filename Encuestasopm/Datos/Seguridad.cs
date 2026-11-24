using System;
using System.Data;
using System.Security.Cryptography;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

namespace Encuestasopm.Datos
{
    public static class Seguridad
    {
        public const int Iteraciones = 100000;

        public static byte[] Hash(string password, byte[] salt, int iteraciones)
        {
            using (var kdf = new Rfc2898DeriveBytes(password, salt, iteraciones, HashAlgorithmName.SHA256))
            {
                return kdf.GetBytes(32);
            }
        }

        public static byte[] NuevoSalt()
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);
            return salt;
        }

        private static bool Iguales(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            int dif = 0;
            for (int i = 0; i < a.Length; i++) dif |= a[i] ^ b[i];
            return dif == 0;
        }

        /// <summary>Valida usuario y contraseña. Devuelve null si no son correctos.</summary>
        public static UsuarioPanel Validar(string usuario, string password)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrEmpty(password)) return null;
            var dt = Db.Consultar("dbo.usp_Usuario_Obtener", Db.P("@usuario", SqlDbType.NVarChar, usuario.Trim(), 50)).Tables[0];
            if (dt.Rows.Count == 0) return null;

            var r = dt.Rows[0];
            var esperado = (byte[])r["passhash"];
            var calculado = Hash(password, (byte[])r["salt"], Convert.ToInt32(r["iteraciones"]));
            if (!Iguales(esperado, calculado)) return null;

            var u = new UsuarioPanel { Id = Convert.ToInt32(r["idusuario"]), Usuario = Convert.ToString(r["usuario"]), Nombre = Convert.ToString(r["nombre"]) };
            Db.Ejecutar("dbo.usp_Usuario_RegistrarAcceso", Db.P("@idusuario", SqlDbType.Int, u.Id));
            return u;
        }

        public static void CambiarPassword(int idusuario, string nueva)
        {
            var salt = NuevoSalt();
            Db.Ejecutar("dbo.usp_Usuario_CambiarPassword",
                Db.P("@idusuario", SqlDbType.Int, idusuario),
                Db.P("@passhash", SqlDbType.VarBinary, Hash(nueva, salt, Iteraciones), 32),
                Db.P("@salt", SqlDbType.VarBinary, salt, 16),
                Db.P("@iteraciones", SqlDbType.Int, Iteraciones));
        }

        /// <summary>Crea la cookie de sesión del panel. En UserData va "id|nombre".</summary>
        public static void IniciarSesion(UsuarioPanel u)
        {
            var ticket = new FormsAuthenticationTicket(1, u.Usuario, DateTime.Now, DateTime.Now.AddHours(8), false,
                u.Id + "|" + u.Nombre, FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static UsuarioPanel UsuarioActual()
        {
            var id = HttpContext.Current.User == null ? null : HttpContext.Current.User.Identity as FormsIdentity;
            if (id == null || !id.IsAuthenticated) return null;
            var partes = (id.Ticket.UserData ?? "").Split(new[] { '|' }, 2);
            int idusuario;
            int.TryParse(partes[0], out idusuario);
            return new UsuarioPanel { Id = idusuario, Usuario = id.Name, Nombre = partes.Length > 1 ? partes[1] : id.Name };
        }

        /// <summary>
        /// Limitador simple por IP en memoria: devuelve false cuando la IP ya hizo
        /// "maximo" intentos de "accion" dentro de la ventana de tiempo.
        /// </summary>
        public static bool PermitirIntento(string accion, int maximo, TimeSpan ventana)
        {
            string ip = HttpContext.Current.Request.UserHostAddress ?? "?";
            string llave = "lim:" + accion + ":" + ip;
            var cont = HttpRuntime.Cache[llave] as int[];
            if (cont == null)
            {
                cont = new[] { 0 };
                HttpRuntime.Cache.Insert(llave, cont, null, DateTime.UtcNow.Add(ventana), Cache.NoSlidingExpiration);
            }
            lock (cont)
            {
                if (cont[0] >= maximo) return false;
                cont[0]++;
                return true;
            }
        }
    }

    public class UsuarioPanel
    {
        public int Id;
        public string Usuario;
        public string Nombre;
    }
}
