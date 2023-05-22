using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitorApi.Data;
using MonitorApi.Metodos.Validacion;
using MonitorApi.Models.DataBase;

namespace MonitorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametrosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParametrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("jobmonitor")]
        public async Task<ActionResult<IEnumerable<JobMonitor>>> GetJobMonitor()
        {
            if (_context.JobMonitors == null)
            {
                return NotFound();
            }
            return await _context.JobMonitors.ToListAsync();
        }


        [HttpPut("jobmonitor/{id}")]
        public async Task<IActionResult> PutJobMonitor(int id, JobMonitor job)
        {
            if (id != job.JobMonitorID) return BadRequest();

            var rest = await ParametroValidacion.ExistsJob(_context, job.JobMonitorID);
            if (!rest.Rest) return Problem(rest.Error);

            rest = await ParametroValidacion.DuplicidadJob(_context, job, true);
            if (rest.Rest) return Problem(rest.Error);

            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }


        [HttpPost("jobmonitor")]
        public async Task<ActionResult<JobMonitor>> PostJobMonitor(JobMonitor job)
        {
            if (_context.JobMonitors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JobMonitors'  is null.");
            }

            var rest = await ParametroValidacion.DuplicidadJob(_context, job);
            if (rest.Rest)
            {
                return Problem(rest.Error);
            }

            _context.JobMonitors.Add(job);
            await _context.SaveChangesAsync(); 

            return NoContent();
        }


        [HttpGet("grupomonitor")]
        public async Task<ActionResult<IEnumerable<Agrupacion>>> GetGrupoMonitor()
        {
            if (_context.Agrupacions == null)
            {
                return NotFound();
            }
            return await _context.Agrupacions.ToListAsync();
        }


        [HttpPut("grupomonitor/{id}")]
        public async Task<IActionResult> PutGrupoMonitor(int id, Agrupacion agrupacion)
        {
            if (id != agrupacion.AgrupacionID) return BadRequest();

            var rest = await ParametroValidacion.ExistsAgrupacion(_context, agrupacion.AgrupacionID);
            if (!rest.Rest) return Problem(rest.Error);

            rest = await ParametroValidacion.DuplicidadAgrupacion(_context, agrupacion, true);
            if (rest.Rest) return Problem(rest.Error);

            _context.Entry(agrupacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPost("grupomonitor")]
        public async Task<ActionResult<JobMonitor>> PostGrupoMonitor(Agrupacion agrupacion)
        {
            if (_context.Agrupacions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JobMonitors'  is null.");
            }

            var rest = await ParametroValidacion.DuplicidadAgrupacion(_context, agrupacion);
            if (rest.Rest)
            {
                return Problem(rest.Error);
            }

            _context.Agrupacions.Add(agrupacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Parametros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Monitore>>> GetMonitorsParametros()
        {
            if (_context.Monitores == null)
            {
                return NotFound();
            }
            return await _context.Monitores.ToListAsync();
        }

        // GET: api/Parametros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobMonitor>> GetJobMonitor(int id)
        {
            if (_context.JobMonitors == null)
            {
                return NotFound();
            }
            var jobMonitor = await _context.JobMonitors.FindAsync(id);

            if (jobMonitor == null)
            {
                return NotFound();
            }

            return jobMonitor;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonitore(int id, Monitore monitor)
        {
            if (id != monitor.MonitoreID) return BadRequest();

            var rest = await ParametroValidacion.ExistsMonitor(_context, monitor.MonitoreID);
            if (!rest.Rest) return Problem(rest.Error);

            rest = await ParametroValidacion.DuplicidadMonitor(_context, monitor, true);
            if (rest.Rest) return Problem(rest.Error);

            _context.Entry(monitor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Monitore>> PostMonitore(Monitore monitor)
        {
            if (_context.Monitores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Monitores'  is null.");
            }

            var rest = await ParametroValidacion.DuplicidadMonitor(_context, monitor);
            if (rest.Rest)
            {
                return Problem(rest.Error);
            }

            _context.Monitores.Add(monitor);
            await _context.SaveChangesAsync();

            return NoContent();
        }





    }
}
