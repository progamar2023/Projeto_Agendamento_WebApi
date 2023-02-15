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
    public class ConfiguracoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConfiguracoesController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Configuracoes>>> GetConfiguracoes()
        {
            return await _context.Configuracoes.ToListAsync();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Configuracoes>> GetConfiguracoes(int id)
        {
            var configuracoes = await _context.Configuracoes.FindAsync(id);

            if (configuracoes == null)
            {
                return NotFound();
            }

            return configuracoes;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutConfiguracoes(int id, Configuracoes configuracoes)
        {
            if (id != configuracoes.Id)
            {
                return BadRequest();
            }

            _context.Entry(configuracoes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfiguracoesExists(id))
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
        public async Task<ActionResult<Configuracoes>> PostConfiguracoes(Configuracoes configuracoes)
        {
            _context.Configuracoes.Add(configuracoes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConfiguracoes", new { id = configuracoes.Id }, configuracoes);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfiguracoes(int id)
        {
            var configuracoes = await _context.Configuracoes.FindAsync(id);
            if (configuracoes == null)
            {
                return NotFound();
            }

            _context.Configuracoes.Remove(configuracoes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConfiguracoesExists(int id)
        {
            return _context.Configuracoes.Any(e => e.Id == id);
        }
    }
}
