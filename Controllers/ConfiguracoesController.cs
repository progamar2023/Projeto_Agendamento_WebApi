using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebApi.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracoesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        public ConfiguracoesController(AppDbContext context, IHostingEnvironment env, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _environment = env;
            _contextAccessor = contextAccessor;
        }

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
        public async Task<ActionResult<IEnumerable<ConfiguracoesRequest>>> GetConfiguracoes()
        {

            var configuracoes =  await _context.Configuracoes.ToListAsync();
            List<ConfiguracoesRequest> result = new List<ConfiguracoesRequest>();
            foreach (var configuracao in configuracoes)
            {
                var imagem = "";
                if (configuracao.Valor != null)
                {
                    var verificaImagem = GetImage(configuracao.Valor);
                    imagem = verificaImagem == "" ? imagem : verificaImagem;
                  
                }

                ConfiguracoesRequest configuracoesReq = new ConfiguracoesRequest(configuracao.Id, configuracao.Local, configuracao.Posicao, configuracao.Tipo, configuracao.Descricao, configuracao.Valor, configuracao.DataInicial, configuracao.DataFinal, imagem);

                result.Add(configuracoesReq);
            }

            return result;
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
        public async Task<IActionResult> PutConfiguracoes(int id, ConfiguracoesRequest configuracoes)
        {
            if (id != configuracoes.Id)
            {
                return BadRequest();
            }

            if (configuracoes.ImagemBase64 != "" && configuracoes.ImagemBase64 != null)
            {
                var identificador = Guid.NewGuid();
                var nome = identificador + ".png";



                if (GetImage(configuracoes.Valor) != "")
                {
                    var folderPath = System.IO.Path.Combine(_environment.ContentRootPath, "Upload");

                    string filePath = Path.Combine(folderPath, configuracoes.Valor);

                    System.IO.File.Delete(filePath);
                   
                }

                SaveImage(configuracoes.ImagemBase64, nome);
                configuracoes.Valor = nome;
                configuracoes.ImagemBase64 = null;
            }

            Configuracoes configuracoesReq = new Configuracoes(configuracoes.Id, configuracoes.Local, configuracoes.Posicao, configuracoes.Tipo, configuracoes.Descricao, configuracoes.Valor, configuracoes.DataInicial, configuracoes.DataFinal);


            _context.Entry(configuracoesReq).State = EntityState.Modified;

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
