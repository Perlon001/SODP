using Microsoft.AspNetCore.Mvc;
using SODP.Domain.Services;
using SODP.Model.Enums;

namespace SODP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArchivalProjectsController : ProjectsController
    {
        private readonly IProjectsService _projectService;

        public ArchivalProjectsController(IProjectsService projectsService) : base(projectsService)
        {
            _projectService.SetArchiveMode();
        }
    }
}