namespace MonitorApi.Models.DataBase
{
    public class MonitorEstadoUltimo
    {
        public int MonitorEstadoUltimoID { get; set; }
        public string PeriodoError { get; set; }
        public DateTime FechaEstado { get; set; }
        public int MonitorID { get; set; }
        public Monitore Monitor { get; set; }
    }
}
