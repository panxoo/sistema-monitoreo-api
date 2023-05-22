namespace MonitorApi.Models.DataBase
{
    public class MonitorEstadoHist
    {
        public int MonitorEstadoHistID { get; set; }

        public bool Estado { get; set; }
        public DateTime Fecha { get; set; }

        public int MonitorID { get; set; }
        public Monitore Monitor { get; set; }
        public bool FalsoPositivo { get; set; }
        public string Nota { get; set; }

        public int ProcesoId { get; set; }
    }
}
