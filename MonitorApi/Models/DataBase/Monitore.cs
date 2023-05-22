using System.ComponentModel.DataAnnotations;
namespace MonitorApi.Models.DataBase
{
    public class Monitore
    {
        public int MonitoreID { get; set; }
        [StringLength(500)]
        [Required]
        public string Nombre { get; set; }

        [StringLength(1000)]
        [Required]
        public string Procedimiento { get; set; }

        [StringLength(5000)]
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public bool Alerta { get; set; }
        [Required]
        public int JobMonitorID { get; set; }

        public JobMonitor? JobMonitor { get; set; }
        public int AgrupacionID { get; set; }
        public Agrupacion? Agrupacion { get; set; }
        public List<MonitorEstadoHist>? MonitorEstadoHists { get; set; }

        public MonitorEstado? MonitorEstado { get; set; }
        public MonitorEstadoUltimo? MonitorEstadoUltimo { get; set; }

        [Required]
        [Range(1, 10000000)]
        public int KeyMonitorProce { get; set; }
    }
}
