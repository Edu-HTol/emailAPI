using MailingApi.Interfaces;
using MailingApi.Layers;
using MailingApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailingController : ControllerBase
    {
        private readonly IBusinessLayer _buisness;
        private readonly ILogger _logger;
        public MailingController(IBusinessLayer buisness, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<MailingController>();
            _buisness = buisness;
        }

        private int GetUserId()
        {
           return int.Parse(User.Identity.Name);
        }

        /// <summary>
        /// Searchs for all groups owned by a user
        /// </summary>
        /// <returns>Returns one group by Id</returns>
        [HttpGet("groups")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User")]
        public IActionResult GetAllGroups()
        {
            var group = _buisness.GetAllBusinessModel(GetUserId()) as List<BusinessModelGroup>;
            if (group is null || group.Count == 0)
            {
                _logger.LogDebug($"{Environment.StackTrace} user: {GetUserId()} - NotFound");
                return NotFound();
            }
            return Ok(group);
        }

        /// <summary>
        /// Searchs for a group by Id
        /// </summary>
        /// <returns>Returns one group by Id</returns>
        [HttpGet("group/{groupId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User")]
        public IActionResult GetGroupById(int groupId)
        {
            var group = _buisness.GetBusinessModel(groupId);
            if (group is null)
            {
                _logger.LogDebug($"{Environment.StackTrace} user: {GetUserId()} - NotFound");
                return NotFound();
            }
            if (group.GroupOwnerId != GetUserId())
            {
                _logger.LogDebug($"{Environment.StackTrace} user: {GetUserId()} - Unauthorized");
                return Unauthorized();
            }
            return Ok(group);
        }
        /// <summary>
        /// Inserts a new group
        /// </summary>
        /// <returns></returns>
        [HttpPost("group")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Roles = "User")]
        public IActionResult PostNewGroup([FromBody]BusinessModelGroup group)
        {
            group.GroupOwnerId = GetUserId();
            var id = _buisness.SaveBusinessModelGroup(group);
            if (id != -1)
            {
                return Created($"{Request.Path}/{id}", id);
            }
            _logger.LogDebug($"{Environment.StackTrace} user: {GetUserId()} - NoContent");
            return NoContent();
        }
        /// <summary>
        /// Updates one group
        /// </summary>
        /// <returns></returns>
        [HttpPut("group/{groupId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "User")]
        public IActionResult PutGroup(int groupId, [FromBody]BusinessModelGroup group)
        {
            group.Id = groupId;
            var actualGroup = _buisness.GetBusinessModel(group.Id);
            if (actualGroup is null)
            {
                _logger.LogDebug($"{Environment.StackTrace} user: {GetUserId()} - NotFound");
                return NotFound();
            }
            if (GetUserId() != actualGroup.GroupOwnerId)
            {
                _logger.LogDebug($"{Environment.StackTrace} user: {GetUserId()} - Unauthorized");
                return Unauthorized();
            }
            group.GroupOwnerId = GetUserId();
            _buisness.PutBusinessModelGroup(group);
            return Ok();
        }
        /// <summary>
        /// Deletes one group
        /// </summary>
        /// <returns></returns>
        [HttpDelete("group/{groupId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "User")]
        public IActionResult DeleteGroup(int groupId)
        {
            var group = _buisness.GetBusinessModel(groupId);
            if(group is null)
            {
                return NotFound();
            }
            if(group.GroupOwnerId != GetUserId())
            {
                _logger.LogDebug($"{Environment.StackTrace} user: {GetUserId()} - Unauthorized");
                return Unauthorized();
            }
            var result = _buisness.DeleteBusinessModelGroup(groupId);
            if(result)
            {
                return Ok();
            }
            _logger.LogDebug($"{Environment.StackTrace} user: {GetUserId()} - NotFound");
            return NotFound();
        }

    }
}
