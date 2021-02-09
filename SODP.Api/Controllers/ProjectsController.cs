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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var req = HttpContext.Request.Query;
            int.TryParse(req["page_number"], out int page_number);
            int.TryParse(req["page_size"], out int page_size);
            return Ok(await _projectsService.GetAllAsync(page_number: page_number, page_size: page_size));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _projectsService.DeleteAsync(id));
        }
    }
}
