namespace MonitorApi.Models.DataBase
{
    public class MonitorEstado
    {
        public int MonitorEstadoID { get; set; }
        public bool Estado { get; set; }
        public DateTime Fecha { get; set; }
        public int MonitorID { get; set; }
        public Monitore Monitor { get; set; }
    }
}
