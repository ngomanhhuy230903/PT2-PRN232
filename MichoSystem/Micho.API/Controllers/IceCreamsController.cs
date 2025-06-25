using Micho.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Micho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IceCreamsController : ControllerBase
    {
        private readonly MichoOrderingSystemContext _context;

        public IceCreamsController(MichoOrderingSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IceCream>>> GetIceCreams()
        {
            return await _context.IceCreams.ToListAsync();
        }
    }
}