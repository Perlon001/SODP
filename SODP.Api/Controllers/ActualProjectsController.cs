using Microsoft.AspNetCore.Mvc;
using SODP.Domain.Services;

namespace SODP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActualProjectsController : ProjectsController
    {
        public ActualProjectsController(IProjectsService projectsService) : base(projectsService) {}
    }
}