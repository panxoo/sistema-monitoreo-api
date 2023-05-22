using Microsoft.EntityFrameworkCore;
using MonitorApi.Models.DataBase;
using MonitorApi.Models.DataBase;

namespace MonitorApi.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options)
        { }

        public DbSet<Monitore> Monitores { get; set; }
        public DbSet<MonitorEstado> MonitorEstados { get; set; }
        public DbSet<JobMonitor> JobMonitors { get; set; }
        public DbSet<Agrupacion> Agrupacions { get; set; }
        public DbSet<MonitorEstadoHist> MonitorEstadoHists { get; set; }
        //public DbSet<ViewHistEstadoMonitor> ViewHistEstadoMonitors { get; set; }
        public DbSet<MonitorEstadoUltimo> MonitorEstadoUltimos { get; set; }
    }
}
