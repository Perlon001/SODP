using Microsoft.AspNetCore.Mvc;
using SODP.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SODP.Api.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DesignerController : ControllerBase
    {
        private readonly IDesignersService _designersService;

        public DesignerController(IDesignersService designersService)
        {
            _designersService = designersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDesigners()
        {
            return Ok(await _designersService.GetAllAsync());
        }
    }
}
