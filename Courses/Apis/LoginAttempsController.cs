using Interfaces.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Apis
{
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginAttempsController : ControllerBase
    {
        private ILoginAttemp _repo;
        public LoginAttempsController(ILoginAttemp repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _repo.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("search/{key}")]
        public async Task<IActionResult> Search(string key)
        {
            try
            {
                var f = await _repo.Search(key);
                return Ok(f);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
