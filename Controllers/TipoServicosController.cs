using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoServicosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoServicosController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoServico>>> GetTipoServicos()
        {
            return await _context.TipoServicos.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoServico>> GetTipoServico(int id)
        {
            var tipoServico = await _context.TipoServicos.FindAsync(id);

            if (tipoServico == null)
            {
                return NotFound();
            }

            return tipoServico;
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoServico(int id, TipoServico tipoServico)
        {
            if (id != tipoServico.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipoServico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoServicoExists(id))
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

        
        [HttpPost]
        public async Task<ActionResult<TipoServico>> PostTipoServico(TipoServico tipoServico)
        {
            _context.TipoServicos.Add(tipoServico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoServico", new { id = tipoServico.Id }, tipoServico);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoServico(int id)
        {
            var tipoServico = await _context.TipoServicos.FindAsync(id);
            if (tipoServico == null)
            {
                return NotFound();
            }

            _context.TipoServicos.Remove(tipoServico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoServicoExists(int id)
        {
            return _context.TipoServicos.Any(e => e.Id == id);
        }
    }
}
