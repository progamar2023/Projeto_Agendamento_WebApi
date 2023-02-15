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
    public class ServicosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServicosController(AppDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public List<ServicoRequest> GetServicos(string? busca = null)
        {
            if(busca == null)
            {
                busca = "";
            }

            List<Servico> Servicos =  _context.Servicos.ToList();
            List<TipoServico> TipoServicos =  _context.TipoServicos.ToList();

            List<ServicoRequest> servicosRequest = new List<ServicoRequest>();

            var query = (from tbs in Servicos
                        join tbts in TipoServicos on tbs.TipoServicoId
                        equals tbts.Id
                        where tbs.Descricao.ToLower().StartsWith(busca.ToLower())
                        || tbs.Nome.ToLower().StartsWith(busca.ToLower())
                         select new ServicoRequest
                        {
                            Servico = tbs,
                            TipoServico = tbts
                        }).ToList();
            foreach(var servico in query)
            {
                servicosRequest.Add(servico);
            }
            return servicosRequest;
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<Servico>> GetServico(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);

            if (servico == null)
            {
                return NotFound();
            }

            return servico;
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServico(int id, Servico servico)
        {
            if (id != servico.Id)
            {
                return BadRequest();
            }

            _context.Entry(servico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(servico);
        }

        
        [HttpPost]
        public async Task<ActionResult<Servico>> PostServico(Servico servico)
        {
            _context.Servicos.Add(servico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServico", new { id = servico.Id }, servico);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServico(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null)
            {
                return NotFound();
            }

            _context.Servicos.Remove(servico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicoExists(int id)
        {
            return _context.Servicos.Any(e => e.Id == id);
        }
    }
}
