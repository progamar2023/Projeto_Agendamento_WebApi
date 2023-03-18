using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Collections.Specialized.BitVector32;
using Microsoft.Extensions.Hosting.Internal;
using System.ComponentModel;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;

        public ServicosController(AppDbContext context, IHostingEnvironment env, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _environment = env;
            _contextAccessor = contextAccessor;
        }


        // [HttpPost("Save")]
        [NonAction]
        public void SaveImage(string base64img, string outputImgFilename = "image.jpg")
        {
            var folderPath = System.IO.Path.Combine(_environment.ContentRootPath, "Upload");
            if (!Directory.Exists(folderPath))
            {
               Directory.CreateDirectory(folderPath);
            }
            System.IO.File.WriteAllBytes(Path.Combine(folderPath, outputImgFilename), Convert.FromBase64String(base64img));
        }

        [HttpGet("GetImage")]
        public string GetImage(string? foto = "")
        {
          
                 var component = foto;
                 var folderPath = System.IO.Path.Combine(_environment.ContentRootPath, "Upload");

                 string filePath = Path.Combine(folderPath, component);

                    if (System.IO.File.Exists(filePath))
                    {

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                fileStream.CopyTo(memoryStream);
                                Bitmap image = new Bitmap(1, 1);
                                image.Save(memoryStream, ImageFormat.Jpeg);

                                byte[] byteImage = memoryStream.ToArray();
                                string value = System.Convert.ToBase64String(byteImage);
                                return value;
                            }
                        }
                    } 
                    else
                    {
                         return "";
                    }
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
                if (servico.Servico.Imagem != null)
                {
                    servico.ImagemBase64 = GetImage(servico.Servico.Imagem);
                }
              
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
        public async Task<IActionResult> PutServico(int id, ServicoImagemRequest servico)
        {

    
            if (id != servico.Id)
            {
                return BadRequest();
            }

            if(servico.ImagemBase64 != null)
            {
              var identificador = Guid.NewGuid();
               var nome = identificador + ".png";
             
              

                if (GetImage(servico.Imagem) != "")
                {
                    var folderPath = System.IO.Path.Combine(_environment.ContentRootPath, "Upload");

                    string filePath = Path.Combine(folderPath, servico.Imagem);

                    System.IO.File.Delete(filePath);
                }

                SaveImage(servico.ImagemBase64, nome);

                servico.Imagem = nome;
                servico.ImagemBase64 = null;
            }

            Servico servicosReq = new Servico(servico.Id, servico.Nome, servico.Descricao, servico.Imagem, servico.TipoServicoId, servico.DataCriacao, true);

            _context.Entry(servicosReq).State = EntityState.Modified;

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

            return Ok(servicosReq);
        }

        
        [HttpPost]
        public async Task<ActionResult<Servico>> PostServico(ServicoImagemRequest servico)
        {
           
                var identificador = Guid.NewGuid();
                var nome = identificador + ".png";
                SaveImage(servico.ImagemBase64, nome);

                servico.Imagem = nome;
                servico.ImagemBase64 = null;
                Servico servicosReq = new Servico(servico.Id, servico.Nome, servico.Descricao, nome, servico.TipoServicoId, servico.DataCriacao, true);
                _context.Servicos.Add(servicosReq);

            
           
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServico", new { id = servicosReq.Id }, servicosReq);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServico(int id)
        {

            var servico = await _context.Servicos.FindAsync(id);

            if (GetImage(servico.Imagem) != "")
            {
                var folderPath = System.IO.Path.Combine(_environment.ContentRootPath, "Upload");

                string filePath = Path.Combine(folderPath, servico.Imagem);

                System.IO.File.Delete(filePath);
            }
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
