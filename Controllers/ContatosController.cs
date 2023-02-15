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
    public class ContatosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContatosController(AppDbContext context)
        {
            _context = context;
        }

        //Lista os Contatos
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contato>>> GetContatos([FromRoute] DateTime? dataInicial = null, [FromRoute] DateTime? dataFinal = null)
        {
            return await _context.Contatos.Where(x => x.DataCriacao >= (dataInicial ?? DateTime.MinValue)
                                                   && x.DataCriacao <= (dataFinal ?? DateTime.MaxValue)).ToListAsync();
        }

        //Mostra detalhes de um contato
        [HttpGet("{id}")]
        public async Task<ActionResult<Contato>> GetContato(int id)
        {
            var contato = await _context.Contatos.FindAsync(id);

            if (contato == null)
            {
                return NotFound();
            }

            return contato;
        }

        //Cadastra um contato
        [HttpPost]
        public async Task<ActionResult<Contato>> PostContato(Contato contato)
        {
            _context.Contatos.Add(contato);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContato", new { id = contato.Id }, contato);
        }

    }
}
