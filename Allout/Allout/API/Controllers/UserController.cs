using Allout.API.Core.Models;
using Allout.BusinessLogic.Core.Interfaces;
using Allout.Core;
using AutoMapper;
using BusinessLogic.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Share.Exceptions;

namespace Allout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UserInformationDto>> Registration(UserIdentityDto userIdentityDto)
        {
            UserIdentityBlo userIdentityBlo = _mapper.Map<UserIdentityBlo>(userIdentityDto);
            UserInformationBlo userInformationBlo;

            try
            {
                userInformationBlo = await _userService.Registration(userIdentityBlo);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }

            return Created("", _mapper.Map<UserInformationDto>(userInformationBlo));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UserInformationDto>> Authenticate(UserIdentityDto userIdentityDto)
        {
            UserIdentityBlo userIdentityBlo = _mapper.Map<UserIdentityBlo>(userIdentityDto);
            UserInformationBlo userInformationBlo;

            try
            {
                userInformationBlo = await _userService.Authenticate(userIdentityBlo);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(_mapper.Map<UserInformationDto>(userInformationBlo));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserInformationDto>> GetUser(int userId)
        {
            UserInformationBlo userInformationBlo;

            try
            {
                userInformationBlo = await _userService.GetUser(userId);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(_mapper.Map<UserInformationDto>(userInformationBlo));
        }

        [HttpPatch("{nickname}")]
        public async Task<ActionResult<UserInformationDto>> UpdateUser([FromRoute] string nickname, [FromBody] UserUpdateDto userUpdateDto)
        {
            UserUpdateBlo userUpdateBlo = _mapper.Map<UserUpdateBlo>(userUpdateDto);
            UserInformationBlo userInformationBlo;

            try
            {
                userInformationBlo = await _userService.UpdateUser(nickname, userUpdateBlo);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(_mapper.Map<UserInformationDto>(userInformationBlo));
        }

        [HttpPatch("[action]/{nickname}")]
        public async Task<ActionResult<string>> UpdateAvatarUser([FromRoute] string nickname, [FromBody] UserUpdateAvatarDto userUpdateAvatarDto)
        {
            string newAvatarUrl = string.Empty;
            try
            {
                newAvatarUrl = await _userService.UpdateAvatarUser(nickname, userUpdateAvatarDto.avatarUrl);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(newAvatarUrl);
        }

        [HttpPost("[action]/{userId}")]
        public async Task<ActionResult<UserInformationDto>> MarkAsDeletedUser(int userId)
        {
            UserInformationBlo userInformationBlo;

            try
            {
                userInformationBlo = await _userService.MarkAsDeletedUser(userId);
            }
            catch (BadRequestException e)
            {
                return NotFound(e.Message);
            }

            return Ok(_mapper.Map<UserInformationDto>(userInformationBlo));
        }

        [HttpGet("[action]/{nickname}")]
        public async Task<ActionResult<bool>> DoesExistUser(string nickname)
        {
            return await _userService.DoesExistUser(nickname);
        }
    }
}
