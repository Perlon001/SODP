using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SODP.Domain.Services;
using SODP.Model.Enums;

namespace SODP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArchiveProjectsController : ProjectsController
    {
        public ArchiveProjectsController(IProjectsService projectsService) : base(projectsService)
        {
            _projectsService.SetArchiveMode();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            return Ok(await _projectsService.RestoreAsync(id));
        }
    }
}