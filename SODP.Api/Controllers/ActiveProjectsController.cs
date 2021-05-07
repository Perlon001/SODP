using Microsoft.AspNetCore.Mvc;
using SODP.Domain.Services;

namespace SODP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActiveProjectsController : ProjectsController
    {
        public ActiveProjectsController(IProjectsService projectsService) : base(projectsService) {}
    }
}