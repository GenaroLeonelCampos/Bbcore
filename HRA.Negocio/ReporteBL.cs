using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRA.Datos;
namespace HRA.Negocio
{
    public class ReporteBL
    {
        public static List<Datos.CONSUMO_PACIENTE_Result> ObtenerPaciente(string hc) 
        {
            using (var db = new BBCORE1Entities())
            {
                return db.CONSUMO_PACIENTE(hc).ToList();
            }
        }
        public static List<Datos.CONSUMO_DETALLE_RESUMEN_Result> ObtenerConsumoDetResum_Paciente(string hc, string fecha_inicio, string fecha_fin)
        {
            using (var db = new BBCORE1Entities())
            {
                DateTime _fecha_inicio = Convert.ToDateTime(fecha_inicio);
                DateTime _fecha_fin = Convert.ToDateTime(fecha_fin);
                return db.CONSUMO_DETALLE_RESUMEN(hc, _fecha_inicio, _fecha_fin).ToList();
            }
        }
        public static List<Datos.CONSUMO_RESUMEN_DONANTES_Result> ObtenerConsumoResum_Donantes(string hc, string fecha_inicio, string fecha_fin)
        {
            using (var db = new BBCORE1Entities())
            {
                DateTime _fecha_inicio = Convert.ToDateTime(fecha_inicio);
                DateTime _fecha_fin = Convert.ToDateTime(fecha_fin);
                return db.CONSUMO_RESUMEN_DONANTES(hc, _fecha_inicio, _fecha_fin).ToList();
            }
        }
        public static List<Datos.CONSUMO_DETALLE_Result> ObtenerConsumoDetalle_Paciente(string hc, string fecha_inicio, string fecha_fin)
        {
            using (var db = new BBCORE1Entities())
            {
                DateTime _fecha_inicio = Convert.ToDateTime(fecha_inicio);
                DateTime _fecha_fin = Convert.ToDateTime(fecha_fin);
                return db.CONSUMO_DETALLE(hc, _fecha_inicio, _fecha_fin).ToList();
            }
        }
        public static List<Datos.ups_DATOS_PACIENTE_Result> ListarDatosPaciente(string hc)
        {
            using (var db = new BBCORE1Entities())
            {
                return db.ups_DATOS_PACIENTE(hc).ToList();
            }
        }
        public static List<Datos.usp_CANT_GLOBULOS_TRANSFUSION_PACIENTE_Result> ObtenerGlobTransf(string hc)
        {
            using (var db = new BBCORE1Entities())
            {
                return db.usp_CANT_GLOBULOS_TRANSFUSION_PACIENTE(hc).ToList();
            }
        }
        public static List<Datos.usp_TRANSFUSIONES_PACIENTE_Result> ObtenerTransfPacient(string hc)
        {
            using (var db = new BBCORE1Entities())
            {
                return db.usp_TRANSFUSIONES_PACIENTE(hc).ToList();
            }
        }
        public static List<Datos.usp_CANDIDATOS_DONACIONES_PACIENTE_Result> ObtenerCandidatos(string hc)
        {
            using (var db = new BBCORE1Entities())
            {
                return db.usp_CANDIDATOS_DONACIONES_PACIENTE(hc).ToList();
            }
        }
    }
}
