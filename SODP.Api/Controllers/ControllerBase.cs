using Microsoft.AspNetCore.Mvc;
using SODP.Domain.DTO;
using SODP.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SODP.Api.Controllers
{
    public abstract class ControllerBase<T> : ControllerBase where T : IEntityService<BaseDTO>
    {
        protected readonly T _service;
        public ControllerBase(T service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync(1, 15));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int Id)
        {
            return Ok(await _service.GetAsync(Id));
        }
    }
}
