using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitorApi.Data;
using MonitorApi.Models.DataBase;
using MonitorApi.Models.Reponses;

namespace MonitorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MonitoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Monitores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MonitorView>>> GetMonitores()
        {
            return await _context.Monitores.Where(w => w.Activo).Select(s => new MonitorView
            {
                MonitorId = s.MonitoreID,
                Title = s.Nombre,
                Description = s.Descripcion,
                Job = s.JobMonitor.Nombre,
                GrupoId = s.AgrupacionID,
                Alerta = s.Alerta
            }).ToListAsync();
        }

        // GET: api/Monitores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Monitore>> GetMonitore(int id)
        {
            if (_context.Monitores == null)
            {
                return NotFound();
            }
            var monitore = await _context.Monitores.FindAsync(id);

            if (monitore == null)
            {
                return NotFound();
            }

            return monitore;
        }

        // PUT: api/Monitores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonitore(int id, Monitore monitore)
        {
            if (id != monitore.MonitoreID)
            {
                return BadRequest();
            }

            _context.Entry(monitore).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonitoreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Monitores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Monitore>> PostMonitore(Monitore monitore)
        {
            if (_context.Monitores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Monitores'  is null.");
            }
            _context.Monitores.Add(monitore);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMonitore", new { id = monitore.MonitoreID }, monitore);
        }

        // DELETE: api/Monitores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonitore(int id)
        {
            if (_context.Monitores == null)
            {
                return NotFound();
            }
            var monitore = await _context.Monitores.FindAsync(id);
            if (monitore == null)
            {
                return NotFound();
            }

            _context.Monitores.Remove(monitore);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MonitoreExists(int id)
        {
            return (_context.Monitores?.Any(e => e.MonitoreID == id)).GetValueOrDefault();
        }
    }
}
