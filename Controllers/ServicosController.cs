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

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _environment;

        public ServicosController(AppDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _environment = env;
        }

        /*
        [HttpPost]
        public void SaveImage(string base64img, string outputImgFilename = "image.jpg")
        {
            var folderPath = System.IO.Path.Combine(_env.ContentRootPath, "Upload");
            if (!Directory.Exists(folderPath))
            {
               Directory.CreateDirectory(folderPath);
            }
            System.IO.File.WriteAllBytes(Path.Combine(folderPath, outputImgFilename), Convert.FromBase64String(base64img));
        }

        [Route("api/dashboard/GetImage")]
        public byte[] GetImage(int componentId)
        {
            using (var dashboardService = new Servico())
            {
                var component = dashboardService.GetImage(componentId);
                var context = HttpContext.Current;
                string filePath = context.Server.MapPath("~/Images/" + component.ImageName);
                context.Response.ContentType = "image/jpeg";
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                        Bitmap image = new Bitmap(1, 1);
                        image.Save(memoryStream, ImageFormat.Jpeg);

                        byte[] byteImage = memoryStream.ToArray();
                        return byteImage;
                    }
                }
            }
        }

        */

        [NonAction]
        private string GetFilePath(string codigo)
        {
            return this._environment.WebRootPath + "\\Upload\\" + codigo + DateTime.Now;
        }

        [HttpPost("UploadImage")]
        public async Task<ActionResult> UploadImage()
        {
            bool Results = false;
            try
            {
                var _uploadedfiles = Request.Form.Files;
                foreach (IFormFile source in _uploadedfiles)
                {
                    string Filename = source.FileName;
                    string Filepath = GetFilePath(Filename);

                    if (!System.IO.Directory.Exists(Filepath))
                    {
                        System.IO.Directory.CreateDirectory(Filepath);
                    }

                    string imagepath = Filepath + "\\image.png";

                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagepath))
                    {
                        await source.CopyToAsync(stream);
                        Results = true;
                    }


                }
            }
            catch (Exception ex)
            {

            }
            return Ok(Results);
        }


        [NonAction]
        private string GetImagebyProduct(string productcode)
        {
            string ImageUrl = string.Empty;
            string HostUrl = "https://localhost:7118/";
            string Filepath = GetFilePath(productcode);
            string Imagepath = Filepath + "\\image.png";
            if (!System.IO.File.Exists(Imagepath))
            {
                ImageUrl = HostUrl + "/uploads/common/noimage.png";
            }
            else
            {
                ImageUrl = HostUrl + "/upload/" + productcode + DateTime.Now + "/image.png";
            }
            return ImageUrl;

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
