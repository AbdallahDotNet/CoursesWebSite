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
    public class BookingsController : ControllerBase
    {
        private IBooking _repo;
        public BookingsController(IBooking repo)
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
        public IActionResult Search(string key)
        {
            try
            {
                return Ok(_repo.Search(key));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
