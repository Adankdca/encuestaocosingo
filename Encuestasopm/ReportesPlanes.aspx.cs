using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace Encuestasopm
{
    public partial class ReportesPlanes : System.Web.UI.Page
    {
        Encuesta.Cnn cn;
        ReportDocument oRpt = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            String IDAnio;
            String Nombre;
            String Reporte;
            //String Annio;

            //IDAnio = (String)Session["xIDAnio"];
            Nombre = (String)Session["xNombre"];
            Reporte = (String)Session["xNreporte"];
            //Nombre= "Reportes\\Rptplanes.rpt";
            //Reporte = "1";
            //Annio = (String)Session["xAnnio"];

            if (Reporte == "1")
            {

                DataRow dr;// = new DataRow();
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                DataSet dsx = new DataSet();

                dt.Columns.Add("nombre", System.Type.GetType("System.String"));
                dt.Columns.Add("cantidad", System.Type.GetType("System.Int64"));


                cn = new Encuesta.Cnn();
                string[,] pArreglorB = new string[,] { { "@idanio", "8", "2022" } };
                ds = cn.funGetbyStoresPropios("Get_Rpt_Planes", pArreglorB);

                Planes iDS;
                iDS = new Planes();

                //dsIndicadorAvance iDS;
                //iDS = new  dsIndicadorAvance();


                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    foreach (DataRowView dataRowCurrent in dataView)
                    {
                        dr = dt.NewRow();
                        dr["nombre"] = dataRowCurrent["nombre"].ToString();
                        dr["cantidad"] = Convert.ToInt64(dataRowCurrent["cantidad"].ToString());

                        dt.Rows.Add(dr);
                    }
                    dsx.Tables.Add(dt);
                    dsx.Tables[0].TableName = "Planes";
                    iDS.Merge(dsx, false, System.Data.MissingSchemaAction.Ignore);

                }


                oRpt = new ReportDocument();
                oRpt.Load(HttpContext.Current.ApplicationInstance.Server.MapPath(Nombre));

                oRpt.SetDataSource(iDS);
                oRpt.PrintOptions.PaperSize = PaperSize.PaperLetter;

            }
            if (Reporte == "2")
            {

                DataRow dr;// = new DataRow();
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                DataSet dsx = new DataSet();

                dt.Columns.Add("nombre", System.Type.GetType("System.String"));
                dt.Columns.Add("cantidad", System.Type.GetType("System.Int64"));


                cn = new Encuesta.Cnn();
                string[,] pArreglorB = new string[,] { { "@idanio", "8", "2022" } };
                ds = cn.funGetbyStoresPropios("Get_Rpt_Temas", pArreglorB);

                Planes iDS;
                iDS = new Planes();

                //dsIndicadorAvance iDS;
                //iDS = new  dsIndicadorAvance();


                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    foreach (DataRowView dataRowCurrent in dataView)
                    {
                        dr = dt.NewRow();
                        dr["nombre"] = dataRowCurrent["nombre"].ToString();
                        dr["cantidad"] = Convert.ToInt64(dataRowCurrent["cantidad"].ToString());

                        dt.Rows.Add(dr);
                    }
                    dsx.Tables.Add(dt);
                    dsx.Tables[0].TableName = "Temas";
                    iDS.Merge(dsx, false, System.Data.MissingSchemaAction.Ignore);

                }


                oRpt = new ReportDocument();
                oRpt.Load(HttpContext.Current.ApplicationInstance.Server.MapPath(Nombre));

                oRpt.SetDataSource(iDS);
                oRpt.PrintOptions.PaperSize = PaperSize.PaperLetter;

            }
            //====================================se cambio para se pueda ver en linea el reporte======================================
            ExportRequestContext req;
            System.IO.Stream stm = null;
            byte[] byteArray = null;


            req = new ExportRequestContext();
            oRpt.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            oRpt.ExportOptions.FormatOptions = new PdfRtfWordFormatOptions();

            req.ExportInfo = oRpt.ExportOptions;
            stm = oRpt.FormatEngine.ExportToStream(req);
            byteArray = new byte[stm.Length];
            stm.Read(byteArray, 0, Convert.ToInt32(stm.Length - 1));
            //====================================se cambio para se pueda ver en linea el reporte======================================

            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "inline; filename=Report.pdf");
            HttpContext.Current.Response.BinaryWrite(byteArray);
            HttpContext.Current.Response.End();



            oRpt.Close();
            oRpt.Dispose();
            GC.Collect();

        }
    }
}