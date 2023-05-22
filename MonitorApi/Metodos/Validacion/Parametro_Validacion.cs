using Microsoft.EntityFrameworkCore;
using MonitorApi.Data;
using MonitorApi.Models.DataBase;
using MonitorApi.Models.Sistema;

namespace MonitorApi.Metodos.Validacion
{
    public static class ParametroValidacion
    {
        public static async Task<RestValidation> DuplicidadMonitor(ApplicationDbContext context, Monitore dato, bool updt = false)
        {
            return await context.Monitores.AnyAsync(a => (!updt || a.MonitoreID != dato.MonitoreID) &&
                                                        (a.Nombre.Equals(dato.Nombre) || a.KeyMonitorProce.Equals(dato.KeyMonitorProce))) ?
                new RestValidation { Rest = true, Error = "Error Monitor o Keys Duplicado" } : new RestValidation { Rest = false };
        }
        public static async Task<RestValidation> DuplicidadJob(ApplicationDbContext context, JobMonitor dato, bool updt = false)
        {
            return await context.JobMonitors.AnyAsync(a => (!updt || a.JobMonitorID != dato.JobMonitorID) && (a.Nombre.Equals(dato.Nombre))) ?
                new RestValidation { Rest = true, Error = "Error Job Duplicado" } : new RestValidation { Rest = false };
        }
        public static async Task<RestValidation> DuplicidadAgrupacion(ApplicationDbContext context, Agrupacion dato, bool updt = false)
        {
            return await context.Agrupacions.AnyAsync(a => (!updt || a.AgrupacionID != dato.AgrupacionID) && (a.Nombre.Equals(dato.Nombre))) ?
                new RestValidation { Rest = true, Error = "Error Agrupacion Duplicado" } : new RestValidation { Rest = false };
        }

        public static async Task<RestValidation> ExistsMonitor(ApplicationDbContext context, int id)
        {
            return await context.Monitores.AnyAsync(a => a.MonitoreID.Equals(id)) ?
                new RestValidation { Rest = true } : new RestValidation { Rest = false, Error = "Error No esiste el Monitor" };
        }

        public static async Task<RestValidation> ExistsJob(ApplicationDbContext context, int id)
        {
            return await context.JobMonitors.AnyAsync(a => a.JobMonitorID.Equals(id)) ?
                new RestValidation { Rest = true } : new RestValidation { Rest = false, Error = "Error No existe el Job" };
        }

        public static async Task<RestValidation> ExistsAgrupacion(ApplicationDbContext context, int id)
        {
            return await context.Agrupacions.AnyAsync(a => a.AgrupacionID.Equals(id)) ?
                new RestValidation { Rest = true } : new RestValidation { Rest = false, Error = "Error No esiste la agrupacion" };
        }

    }
}
