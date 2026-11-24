using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Caching;

namespace Encuestasopm.Datos
{
    public class EncuestaNueva
    {
        public int IdPlanes;
        public string Nombre;
        public string Genero;
        public int IdEdad;
        public int IdOcupacion;
        public string Residente;
        public string Zona;
        public string Localidad;
        public string Existencia;
        public string Apoyo;
        public bool Inseguridad, Bienestar, Abandono, Economico, Desatencion, Servicios;
        public string Problematica;
        public string Propuesta;
        public string Archivo;
        public string ArchivoOriginal;
        public int IdConfianza;
    }

    /// <summary>Filtros comunes del panel, del reporte y de la exportación.</summary>
    public class FiltroPanel
    {
        public DateTime? Desde;
        public DateTime? Hasta;
        public int? IdTipo;
        public int? IdPlanes;
        public string Buscar;

        public static FiltroPanel DesdeRequest(HttpRequest req)
        {
            return new FiltroPanel
            {
                Desde = Fecha(req["desde"]),
                Hasta = Fecha(req["hasta"]),
                IdTipo = Entero(req["tipo"]),
                IdPlanes = Entero(req["plan"]),
                Buscar = string.IsNullOrWhiteSpace(req["buscar"]) ? null : req["buscar"].Trim()
            };
        }

        private static DateTime? Fecha(string s)
        {
            DateTime d;
            if (DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d)) return d;
            return null;
        }

        private static int? Entero(string s)
        {
            int n;
            if (int.TryParse(s, out n) && n > 0) return n;
            return null;
        }
    }

    public static class EncuestaDatos
    {
        private const string LlaveCatalogos = "catalogos-encuesta";

        /// <summary>Catálogos de la encuesta; se guardan 10 minutos en memoria para no consultar en cada visita.</summary>
        public static Dictionary<string, object> Catalogos()
        {
            var cache = HttpRuntime.Cache[LlaveCatalogos] as Dictionary<string, object>;
            if (cache != null) return cache;

            var ds = Db.Consultar("dbo.SPObtenerCatalogos");
            var cat = new Dictionary<string, object>
            {
                { "planes", Db.ALista(ds.Tables[0]) },
                { "edades", Db.ALista(ds.Tables[1]) },
                { "ocupaciones", Db.ALista(ds.Tables[2]) },
                { "confianza", Db.ALista(ds.Tables[3]) },
                { "temas", Db.ALista(ds.Tables[4]) }
            };
            HttpRuntime.Cache.Insert(LlaveCatalogos, cat, null, DateTime.UtcNow.AddMinutes(10), Cache.NoSlidingExpiration);
            return cat;
        }

        /// <summary>Guarda la encuesta y devuelve el folio asignado.</summary>
        public static string Guardar(EncuestaNueva e)
        {
            var folio = Db.Salida("@folio", SqlDbType.VarChar, 20);
            Db.Ejecutar("dbo.Insert_Encuesta",
                Db.P("@idplanes", SqlDbType.Int, e.IdPlanes),
                Db.P("@Nombre", SqlDbType.NVarChar, e.Nombre, 150),
                Db.P("@Genero", SqlDbType.Char, e.Genero, 1),
                Db.P("@idedad", SqlDbType.Int, e.IdEdad),
                Db.P("@idocupacion", SqlDbType.Int, e.IdOcupacion),
                Db.P("@residente", SqlDbType.Char, e.Residente, 2),
                Db.P("@zona", SqlDbType.Char, e.Zona, 1),
                Db.P("@localidad", SqlDbType.NVarChar, e.Localidad, 150),
                Db.P("@existencia", SqlDbType.Char, e.Existencia, 2),
                Db.P("@apoyo", SqlDbType.Char, e.Apoyo, 2),
                Db.P("@inseguridad", SqlDbType.Bit, e.Inseguridad),
                Db.P("@bienestar", SqlDbType.Bit, e.Bienestar),
                Db.P("@abandono", SqlDbType.Bit, e.Abandono),
                Db.P("@economico", SqlDbType.Bit, e.Economico),
                Db.P("@desatencion", SqlDbType.Bit, e.Desatencion),
                Db.P("@servicios", SqlDbType.Bit, e.Servicios),
                Db.P("@problematica", SqlDbType.NVarChar, e.Problematica, 2000),
                Db.P("@propuesta", SqlDbType.NVarChar, e.Propuesta, 2000),
                Db.P("@archivo", SqlDbType.NVarChar, e.Archivo, 260),
                Db.P("@archivooriginal", SqlDbType.NVarChar, e.ArchivoOriginal, 260),
                Db.P("@idconfianza", SqlDbType.Int, e.IdConfianza),
                Db.Salida("@idpregunta", SqlDbType.Int),
                folio);
            return Convert.ToString(folio.Value);
        }

        public static Dictionary<string, object> Resumen(FiltroPanel f)
        {
            var ds = Db.Consultar("dbo.Get_Panel_Resumen", ParametrosFiltro(f, false));
            var kpi = Db.ALista(ds.Tables[0]);
            return new Dictionary<string, object>
            {
                { "kpi", kpi.Count > 0 ? kpi[0] : new Dictionary<string, object>() },
                { "planes", Db.ALista(ds.Tables[1]) },
                { "temas", Db.ALista(ds.Tables[2]) },
                { "confianza", Db.ALista(ds.Tables[3]) },
                { "edades", Db.ALista(ds.Tables[4]) },
                { "genero", Db.ALista(ds.Tables[5]) },
                { "ocupaciones", Db.ALista(ds.Tables[6]) },
                { "meses", Db.ALista(ds.Tables[7]) },
                { "localidades", Db.ALista(ds.Tables[8]) }
            };
        }

        public static DataTable Listado(FiltroPanel f, int? top)
        {
            var ps = new List<System.Data.SqlClient.SqlParameter>(ParametrosFiltro(f, true));
            ps.Add(Db.P("@top", SqlDbType.Int, top.HasValue ? (object)top.Value : null));
            return Db.Consultar("dbo.Get_Encuestas_Listado", ps.ToArray()).Tables[0];
        }

        private static System.Data.SqlClient.SqlParameter[] ParametrosFiltro(FiltroPanel f, bool conBusqueda)
        {
            var ps = new List<System.Data.SqlClient.SqlParameter>
            {
                Db.P("@desde", SqlDbType.Date, f.Desde),
                Db.P("@hasta", SqlDbType.Date, f.Hasta),
                Db.P("@idtipo", SqlDbType.Int, f.IdTipo),
                Db.P("@idplanes", SqlDbType.Int, f.IdPlanes)
            };
            if (conBusqueda) ps.Add(Db.P("@buscar", SqlDbType.NVarChar, f.Buscar, 100));
            return ps.ToArray();
        }

        /// <summary>Devuelve (nombre guardado, nombre original) del adjunto de una encuesta, o null.</summary>
        public static string[] Adjunto(int idpregunta)
        {
            var dt = Db.Consultar("dbo.Get_Encuesta_Archivo", Db.P("@idpregunta", SqlDbType.Int, idpregunta)).Tables[0];
            if (dt.Rows.Count == 0 || dt.Rows[0]["archivo"] == DBNull.Value) return null;
            return new[] { Convert.ToString(dt.Rows[0]["archivo"]), Convert.ToString(dt.Rows[0]["archivooriginal"]) };
        }
    }
}
