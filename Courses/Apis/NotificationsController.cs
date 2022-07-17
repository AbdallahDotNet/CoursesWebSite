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
    public class NotificationsController : ControllerBase
    {
        private INotification _repo;
        public NotificationsController(INotification repo)
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

        [HttpGet("update-seen")]
        public async Task<IActionResult> UpdateSeen()
        {
            try
            {
                await _repo.UpdateSeen();
                return Ok(await _repo.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("get-unSeen-count")]
        public IActionResult GetUnSeenCount()
        {
            try
            {
                return Ok(_repo.GetNotificationUnSeenCount());
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
