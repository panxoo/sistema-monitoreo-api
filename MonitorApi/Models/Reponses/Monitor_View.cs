namespace MonitorApi.Models.Reponses
{
    public class MonitorView
    {
        public int MonitorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Job { get; set; }
        public int GrupoId { get; set; }
        public bool Alerta { get; set; }
    }
}
