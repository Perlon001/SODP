using Microsoft.AspNetCore.Mvc;
using SODP.DataAccess;
using SODP.Domain.Services;
using System.Threading.Tasks;

namespace SODP.Api.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StagesController : ControllerBase
    {
        private readonly IStagesService _stagesService;

        public StagesController(IStagesService stagesService)
        {
            _stagesService = stagesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _stagesService.GetAllAsync());
        }

        //[AllowAnonymous]
        //[HttpGet]
        //public async Task<ActionResult<PageResponse<Stage>>> GetStages()
        //{
        //    // sample code with id from token
        //    //var claim = User.Claims.FirstOrDefault(x => x.Type == "id");
        //    //if((claim == null) || !int.TryParse(claim.Value, out int userId))
        //    //{
        //    //    return BadRequest();
        //    //}

        //    return Ok(await _stagesService.GetAll());
        //}

        //[AllowAnonymous]
        //[HttpGet("{sign}")]
        //public async Task<ActionResult<Stage>> Get(string sign)
        //{
        //    return Ok(await _stagesService.Get(sign));
        //}

        //[AllowAnonymous]
        //[ValidationFilter]
        //[HttpPost]
        //public async Task<ActionResult<Stage>> Create([FromBody] StageDTO stage)
        //{
        //    return Ok(await _stagesService.Create(stage));
        //}

        //[AllowAnonymous]
        //[ValidationFilter]
        //[HttpPut("{sign}")]
        //public async Task<ActionResult<Stage>> Update(string sign, [FromBody] StageDTO stage)
        //{
        //    if (sign != stage.Sign)
        //    {
        //        return BadRequest();
        //    }
        //    var response = await _stagesService.Update(sign, stage);
        //    if (!response.Success)
        //    {
        //        return BadRequest(response);
        //    }

        //    return Ok(response);
        //}

        //[AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _stagesService.DeleteAsync(id));
        }

    }
}
