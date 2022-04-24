using Allout.API.Core.Models;
using Allout.BusinessLogic.Core.Interfaces;
using Allout.BusinessLogic.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Share.Exceptions;
using Shared.Exceptions;

namespace Allout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyLotController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBuyLotService _buyLotService;

        public BuyLotController(IMapper mapper, IBuyLotService buyLotService)
        {
            _mapper = mapper;
            _buyLotService = buyLotService;
        }

        [HttpPatch("[action]/{userId}/{auctionId}")]
        public async Task<ActionResult<BuyLotDto>> AddBuy([FromRoute] int userId, [FromRoute] int auctionId, [FromBody] BuyLotCreateDto buyLotCreateDto)
        {
            try
            {
                BuyLotCreateBlo byuLotCreateBlo = ConvertToNewBuyLotBlo(buyLotCreateDto);

                BuyLotBlo buyLotBlo = await _buyLotService.AddBuy(userId, auctionId, byuLotCreateBlo);

                return Ok(ConvertToBuyLotDto(buyLotBlo));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ConflictException e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet("[action]/{auctionId}")]
        public async Task<ActionResult<int>> GetLastBuyUserId(int auctionId)
        {
            int userId;

            try
            {
                userId = await _buyLotService.GetLastBuyUserId(auctionId);
                return Ok(userId);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        private BuyLotCreateBlo ConvertToNewBuyLotBlo(BuyLotCreateDto buyLotCreateDto)
        {
            if (buyLotCreateDto == null)
                throw new ArgumentNullException(nameof(buyLotCreateDto));

            BuyLotCreateBlo buyLotCreateBlo = _mapper.Map<BuyLotCreateBlo>(buyLotCreateDto);

            return buyLotCreateBlo;
        }

        private BuyLotDto ConvertToBuyLotDto(BuyLotBlo buyLotBlo)
        {
            if (buyLotBlo == null)
                throw new ArgumentNullException(nameof(buyLotBlo));

            BuyLotDto auctionDto = _mapper.Map<BuyLotDto>(buyLotBlo);

            return auctionDto;
        }
    }
}
