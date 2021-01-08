using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using ResourcitAPI.Data;
using ResourcitModels.Models;

namespace ResourcitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly ResourcitDbContext _context;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment _webHostEnv;

        public ResourcesController(ResourcitDbContext context, 
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.configuration = configuration;
            _webHostEnv = webHostEnvironment;
        }

        // GET: api/Resources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
        {
            return await _context.Resources.ToListAsync();
        }

        // GET: api/Resources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> GetResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);

            if (resource == null)
            {
                return NotFound();
            }

            return resource;
        }

        // PUT: api/Resources/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResource(int id, Resource resource)
        {
            if (!ResourcitAPI.Security.Authenticate.ValidateKey(Request, configuration.GetValue<string>("Authentication:Keys")))
            {
                return Unauthorized();
            }
            if (id != resource.ResourceId)
            {
                return BadRequest();
            }

            _context.Entry(resource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(id))
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

        private bool Authenticate()
        {
            var allowedKey = configuration.GetValue<string>("Authentication:Keys");
            StringValues key = Request.Headers["Key"];
            if (allowedKey == key)
                return true;
            return false;
        }

        // POST: api/Resources
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Resource>> PostResource(Resource resource)
        {
            if (!ResourcitAPI.Security.Authenticate.ValidateKey(Request, configuration.GetValue<string>("Authentication:Keys")))
            {
                return Unauthorized();
            }
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResource", new { id = resource.ResourceId }, resource);
        }

        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Resource>> DeleteResource(int id)
        {
            //if (!ResourcitAPI.Security.Authenticate.ValidateKey(Request, configuration.GetValue<string>("Authentication:Keys")))
            //{
            //    return Unauthorized();
            //}
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return Ok(resource);
        }

        [HttpPost("UploadFile")]
        public async Task<string> UploadFile(IFormFile file)
        {
            string path = Path.Combine(_webHostEnv.WebRootPath, "images/", file.FileName);
            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "https://" + Request.Host.ToString() + "/images/" + file.FileName;
            //var x = HttpContext;
            //var y = Request;
            //return Path.Combine("http://localhost:8888/Images/", file.FileName);
        }

        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.ResourceId == id);
        }
    }
}
