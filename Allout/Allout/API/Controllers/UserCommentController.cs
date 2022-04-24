using Allout.API.Core.Models;
using Allout.BusinessLogic.Core.Interfaces;
using AutoMapper;
using BusinessLogic.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Share.Exceptions;
using Shared.Exceptions;

namespace Allout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCommentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserCommentService _userCommentService;

        public UserCommentController(IMapper mapper, IUserCommentService userCommentService)
        {
            _mapper = mapper;
            _userCommentService = userCommentService;
        }

        [HttpPatch("[action]/{userWhoSendCommentId}/{userWhoGetCommentId}")]
        public async Task<ActionResult<UserCommentDto>> AddUserComment([FromRoute] int userWhoSendCommentId, [FromRoute] int userWhoGetCommentId, [FromBody] UserCommentAddDto userCommentAddDto)
        {
            UserCommentBlo userCommentBlo;
            try
            {
                userCommentBlo = await _userCommentService.AddUserComment(userWhoSendCommentId, userWhoGetCommentId, userCommentAddDto.Text);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ConflictException e)
            {
                return Conflict(e.Message);
            }
            return Created("", _mapper.Map<UserCommentDto>(userCommentBlo));
        }

        [HttpGet("[action]/{userId}/{count}/{skipCount}")]
        public async Task<ActionResult<List<UserCommentDto>>> GetComments(int userId, int count, int skipCount)
        {
            try
            {
                List<UserCommentBlo> userCommentBlos = await _userCommentService.GetComments(userId, count, skipCount);
                return Ok(ConvertToCommentsDto(userCommentBlos));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<int>> GetCommentAmount(int userId)
        {
            int countComment;

            try
            {
                countComment = await _userCommentService.GetCommentAmount(userId);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(countComment);
        }

        private List<UserCommentDto> ConvertToCommentsDto(List<UserCommentBlo> userCommentBlos)
        {
            if (userCommentBlos == null)
                throw new ArgumentNullException(nameof(userCommentBlos));

            List<UserCommentDto> userCommentDtos = new List<UserCommentDto>();
            for (int i = 0; i < userCommentBlos.Count; i++)
            {
                userCommentDtos.Add(_mapper.Map<UserCommentDto>(userCommentBlos[i]));
            }

            return userCommentDtos;
        }
    }
}
