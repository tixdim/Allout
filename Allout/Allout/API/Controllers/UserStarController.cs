using Allout.API.Core.Models;
using Allout.BusinessLogic.Core.Interfaces;
using Allout.Core;
using AutoMapper;
using BusinessLogic.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Share.Exceptions;
using Shared.Exceptions;

namespace Allout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStarController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserStarService _userStarService;

        public UserStarController(IMapper mapper, IUserStarService userStarService)
        {
            _mapper = mapper;
            _userStarService = userStarService;
        }

        [HttpPatch("[action]/{userWhoSendStarId}/{userWhoGetStarId}")]
        public async Task<IActionResult> AddUserStar([FromRoute] int userWhoSendStarId, [FromRoute] int userWhoGetStarId, [FromBody] UserStarAddDto userStarAddDto)
        {
            try
            {
                await _userStarService.AddUserStar(userWhoSendStarId, userWhoGetStarId, userStarAddDto.Count);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ConflictException e)
            {
                return Conflict(e.Message);
            }
            return Ok();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<int>> GetCountStars(int userId)
        {
            int countStars;

            try
            {
                countStars = await _userStarService.GetCountStars(userId);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(countStars);
        }
    }
}
