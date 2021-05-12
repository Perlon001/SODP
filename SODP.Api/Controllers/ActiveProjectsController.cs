using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SODP.Domain.Services;

namespace SODP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActiveProjectsController : ProjectsController
    {
        public ActiveProjectsController(IProjectsService projectsService) : base(projectsService) {}
    
        [HttpPost("{id}")]
        public async Task<IActionResult> Archive(int id)
        {
            return Ok(await _projectsService.ArchiveAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _projectsService.DeleteAsync(id));
        }
    }
}