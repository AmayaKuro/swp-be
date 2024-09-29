using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swp_be.Data;
using swp_be.Data.Repositories;
using swp_be.Models;
using swp_be.Services;

namespace swp_be.Controllers
{
    [Route("api/koi/[controller]")]
    [ApiController]
    public class KoiController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly KoiService koiService;

        public KoiController(ApplicationDBContext context)
        {
            this._context = context;
            this.koiService = new KoiService(context);
        }

        // GET: api/Koi
        [HttpGet]
        public async Task<ActionResult<Koi>> Get()
        {
            return Ok();
            //return await _context.Kois.Take(10).ToListAsync();
        }

        // GET: api/Koi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Koi>> GetKoi(int id)
        {
            var koi = await _context.Kois.FindAsync(id);

            if (koi == null)
            {
                return NotFound();
            }

            return koi;
        }
    }
}
