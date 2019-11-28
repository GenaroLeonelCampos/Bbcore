using HRA.Negocio;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ReporteController : Controller
    {
        public ActionResult ListarProcedimiento(string pHc)
        {
            var lista = ReporteBL.ListarDatosPaciente(pHc);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReporteMovimientoBbCore(string hc, string fecha_inicio, string fecha_fin)
        {
            var paciente = ReporteBL.ObtenerPaciente(hc);
            var consumo_detalle_resumen = ReporteBL.ObtenerConsumoDetResum_Paciente(hc, fecha_inicio, fecha_fin);
            var consumo_resumen_donantes = ReporteBL.ObtenerConsumoResum_Donantes(hc, fecha_inicio, fecha_fin);
            var consumo_detalle = ReporteBL.ObtenerConsumoDetalle_Paciente(hc, fecha_inicio, fecha_fin);
            var rd = new ReportDataSource("dsConsumo", consumo_detalle);
            //var rd = new ReportDataSource("dsUsuario", data);  
            var parametros = new List<ReportParameter>
            {
                new ReportParameter("HISTORIA", paciente[0].HISTORIA),
                new ReportParameter("PACIENTE", paciente[0].POSTULANTE),
                new ReportParameter("ABO", paciente[0].ABO),
                //consumo_detalle_resumen
                 new ReportParameter("GLOBULOS", consumo_detalle_resumen[0].GLOBULOS.Value.ToString()),
                new ReportParameter("PLAQUETAS", consumo_detalle_resumen[0].PLAQUETAS.Value.ToString()),
                new ReportParameter("PLASMA", consumo_detalle_resumen[0].PLASMA.Value.ToString()),
                new ReportParameter("CRIO", consumo_detalle_resumen[0].CRIO.Value.ToString()),
                new ReportParameter("AFERESIS", consumo_detalle_resumen[0].AFERESIS.Value.ToString()),
                //consumo_resumen_donantes
                new ReportParameter("POSTULANTES", consumo_resumen_donantes[0].POSTULANTES.Value.ToString()),
                new ReportParameter("DONANTE", consumo_resumen_donantes[0].DONANTE.Value.ToString()),
                new ReportParameter("RECHAZADOS", consumo_resumen_donantes[0].RECHAZADOS.Value.ToString()),
                new ReportParameter("RECHAZADOS_SEROLOGIA", consumo_resumen_donantes[0].RECHAZADOS_SEROLOGIA.Value.ToString()),
            };
            return Reporte("PDF", "RptMovimientoBbCore.rdlc", rd, "A4Vertical0.25", parametros);
        }
        public ActionResult ReporteProcPaciente(string hc)
        {
            var pacientes = ReporteBL.ListarDatosPaciente("413646");
            var ObtenerGlobTransf = ReporteBL.ObtenerGlobTransf("P18001049");
            var ObtenerTransfPacient = ReporteBL.ObtenerTransfPacient("P18001049");
            var rd = new ReportDataSource("dsTransfu", ObtenerTransfPacient);

            var parametros = new List<ReportParameter>
            {               
                //Pacientes
                new ReportParameter("t_doc", pacientes[0].t_doc),
                new ReportParameter("num_doc", pacientes[0].num_doc),
                new ReportParameter("num_proc", pacientes[0].num_proc),
                new ReportParameter("Gh", pacientes[0].Gh),
                new ReportParameter("PACIENTE", pacientes[0].PACIENTE),
                //Globulos_Transfusión
                new ReportParameter("GLOBULOS", ObtenerGlobTransf[0].GLOBULOS.Value.ToString()),
                new ReportParameter("TRANSFUSION", ObtenerGlobTransf[0].TRANSFUSION.Value.ToString())
        };
            // return Reporte("PDF", "RptTransfusionBbCore.rdlc", null, "A4Vertical0.25", parametros);(Sin DatasSet)
            return Reporte("PDF", "RptTransfusionBbCore.rdlc", rd, "A4Vertical0.25", parametros);
        }
        public ActionResult ReporteMovimientoBbCore2(string proc, string hc)
        {
            var pacientes = ReporteBL.ListarDatosPaciente(hc);
            var ObtenerGlobTransf = ReporteBL.ObtenerGlobTransf(proc);
            var ObtenerTransfPacient = ReporteBL.ObtenerTransfPacient(proc);
            var ObtenerCandidatos = ReporteBL.ObtenerCandidatos(hc);
            var rd = new ReportDataSource("dsTransfu", ObtenerTransfPacient);
            var rd1 = new ReportDataSource("dsCandidato", ObtenerCandidatos);
            var parametros = new List<ReportParameter>
               {               
                   //Pacientes
                   new ReportParameter("t_doc", pacientes[0].t_doc),
                   new ReportParameter("num_doc", pacientes[0].num_doc),
                   new ReportParameter("num_proc", pacientes[0].num_proc),
                   new ReportParameter("Gh", pacientes[0].Gh),
                   new ReportParameter("PACIENTE", pacientes[0].PACIENTE),
                   //Globulos_Transfusión
                   new ReportParameter("GLOBULOS", ObtenerGlobTransf[0].GLOBULOS.Value.ToString()),
                   new ReportParameter("TRANSFUSION", ObtenerGlobTransf[0].TRANSFUSION.Value.ToString()),
                   new ReportParameter("Fecha", DateTime.Now.ToShortDateString())

           };
            //pasar sin data set//
            // return Reporte("PDF", "RptTransfusionBbCore.rdlc", null, "A4Vertical0.25", parametros);         
            return Reporte("PDF", "RptTransfusionBbCore.rdlc", rd, "A4Vertical0.25", parametros, rd1);
        }
        public ActionResult Reporte(string pTipoReporte, string rdlc, ReportDataSource rds, string pPapel, List<ReportParameter> pParametros = null, ReportDataSource rds1=null)
        {
            var lr = new LocalReport();
            lr.ReportPath = Path.Combine(Server.MapPath("~/Reporte"), rdlc);

            if (rds != null) lr.DataSources.Add(rds);
           //Otro data set
            if (rds1 != null) lr.DataSources.Add(rds1);
            if (pParametros != null) lr.SetParameters(pParametros);

            string reportType = pTipoReporte;
            string mimeType = string.Empty;
            string encoding;
            string fileNameExtension;

            var deviceInfo = ObtenerPapel(pPapel).Replace("[TipoReporte]", pTipoReporte);
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes = null;
            try
            {
                renderedBytes = lr.Render(reportType, deviceInfo, out mimeType, out encoding,
                                                            out fileNameExtension, out streams, out warnings);
            }
            catch (Exception)
            {
            }
            return File(renderedBytes, mimeType);
        }
        private static string ObtenerPapel(string pPapel)
        {
            switch (pPapel)
            {
                case "A4Horizontal":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>11in</PageWidth>" +
                           "  <PageHeight>8.5in</PageHeight>" +
                           "  <MarginTop>0in</MarginTop>" +
                           "  <MarginLeft>0in</MarginLeft>" +
                           "  <MarginRight>0in</MarginRight>" +
                           "  <MarginBottom>0in</MarginBottom>" +
                           "</DeviceInfo>";
                case "A4Vertical":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>8.5in</PageWidth>" +
                           "  <PageHeight>11in</PageHeight>" +
                           "  <MarginTop>0in</MarginTop>" +
                           "  <MarginLeft>0in</MarginLeft>" +
                           "  <MarginRight>0in</MarginRight>" +
                           "  <MarginBottom>0in</MarginBottom>" +
                           "</DeviceInfo>";
                case "A4Horizontal0.25":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>11in</PageWidth>" +
                           "  <PageHeight>8.5in</PageHeight>" +
                           "  <MarginTop>0.25in</MarginTop>" +
                           "  <MarginLeft>0.25in</MarginLeft>" +
                           "  <MarginRight>0.25in</MarginRight>" +
                           "  <MarginBottom>0.25in</MarginBottom>" +
                           "</DeviceInfo>";
                case "A4Vertical0.25":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>8.5in</PageWidth>" +
                           "  <PageHeight>11in</PageHeight>" +
                           "  <MarginTop>0.25in</MarginTop>" +
                           "  <MarginLeft>0.25in</MarginLeft>" +
                           "  <MarginRight>0.25in</MarginRight>" +
                           "  <MarginBottom>0.25in</MarginBottom>" +
                           "</DeviceInfo>";
                case "TicketCaja":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>3.5in</PageWidth>" +
                           "  <PageHeight>5.0in</PageHeight>" +
                           "  <MarginTop>0in</MarginTop>" +
                           "  <MarginLeft>0.1in</MarginLeft>" +
                           "  <MarginRight>0in</MarginRight>" +
                           "  <MarginBottom>0in</MarginBottom>" +
                           "</DeviceInfo>";
                case "VoucherCaja":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>8.5in</PageWidth>" +
                           "  <PageHeight>11in</PageHeight>" +
                           "  <MarginTop>0in</MarginTop>" +
                           "  <MarginLeft>0in</MarginLeft>" +
                           "  <MarginRight>0in</MarginRight>" +
                           "  <MarginBottom>0in</MarginBottom>" +
                           "</DeviceInfo>";
                case "CodigoBarras":
                    return "<DeviceInfo>" +
                           "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                           "  <PageWidth>4.13in</PageWidth>" +
                           "  <PageHeight>2.76in</PageHeight>" +
                           "  <MarginTop>0in</MarginTop>" +
                           "  <MarginLeft>0in</MarginLeft>" +
                           "  <MarginRight>0in</MarginRight>" +
                           "  <MarginBottom>0in</MarginBottom>" +
                           "</DeviceInfo>";

            }

            return "<DeviceInfo>" +
                   "  <OutputFormat>[TipoReporte]</OutputFormat>" +
                   "  <PageWidth>8.5in</PageWidth>" +
                   "  <PageHeight>11in</PageHeight>" +
                   "  <MarginTop>0in</MarginTop>" +
                   "  <MarginLeft>0in</MarginLeft>" +
                   "  <MarginRight>0in</MarginRight>" +
                   "  <MarginBottom>0in</MarginBottom>" +
                   "</DeviceInfo>";
        }
    }
}