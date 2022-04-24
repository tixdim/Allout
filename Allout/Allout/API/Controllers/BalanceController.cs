using Allout.API.Core.Models;
using Allout.BusinessLogic.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Share.Exceptions;

namespace Allout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public BalanceController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [HttpPatch("[action]/{userId}")]
        public async Task<ActionResult<float>> AddUserBalance([FromRoute] int userId, [FromBody] UserBalanceAddDto userBalanceAddDto)
        {
            float money;
            try
            {
                money = await _balanceService.AddUserBalance(userId, userBalanceAddDto.Money);
                return Ok(money);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPatch("[action]/{userId}/")]
        public async Task<ActionResult<AuctionDto>> UpdateUserBalance([FromRoute] int userId, [FromBody] UserBalanceUpdateDto userBalanceUpdateDto)
        {
            float money;
            try
            {
                money = await _balanceService.UpdateUserBalance(userId, userBalanceUpdateDto.ChangeMoney);
                return Ok(money);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
