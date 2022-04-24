using Allout.API.Core.Models;
using Allout.BusinessLogic.Core.Interfaces;
using Allout.BusinessLogic.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Share.Exceptions;

namespace Allout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuctionService _auctionService;

        public AuctionController(IMapper mapper, IAuctionService auctionService)
        {
            _mapper = mapper;
            _auctionService = auctionService;
        }

        [HttpPatch("[action]/{userId}")]
        public async Task<ActionResult<AuctionDto>> AddAuction([FromRoute] int userId, [FromBody] AuctionCreateDto auctionCreateDto)
        {
            AuctionCreateBlo auctionCreateBlo = _mapper.Map<AuctionCreateBlo>(auctionCreateDto);
            AuctionBlo auctionBlo;

            try
            {
                auctionBlo = await _auctionService.AddAuction(userId, auctionCreateBlo);
            }
            catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }

            return Created("", ConvertToAuctionDto(auctionBlo));
        }

        [HttpPatch("[action]/{auctionId}/{NowCost}")]
        public async Task<ActionResult<AuctionDto>> UpdateInfoAuction(int auctionId, float NowCost)
        {
            AuctionBlo auctionBlo;

            try
            {
                auctionBlo = await _auctionService.UpdateInfoAuction(auctionId, NowCost);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(ConvertToAuctionDto(auctionBlo));
        }

        [HttpPatch("[action]/{auctionId}/{auctionStatusModerationId}")]
        public async Task<ActionResult<AuctionDto>> UpdateStatusAuction(int auctionId, int auctionStatusModerationId)
        {
            AuctionBlo auctionBlo;

            try
            {
                auctionBlo = await _auctionService.UpdateStatusAuction(auctionId, auctionStatusModerationId);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(ConvertToAuctionDto(auctionBlo));
        }

        [HttpGet("[action]/{auctionId}")]
        public async Task<ActionResult<AuctionDto>> GetAuction(int auctionId)
        {
            AuctionBlo auctionBlo;

            try
            {
                auctionBlo = await _auctionService.GetAuction(auctionId);
                return Ok(ConvertToAuctionDto(auctionBlo));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("[action]/{auctionId}")]
        public async Task<ActionResult<AuctionDto>> MarkAsDeletedAuction(int auctionId)
        {
            AuctionBlo auctionBlo;

            try
            {
                auctionBlo = await _auctionService.MarkAsDeletedAuction(auctionId);
            }
            catch (BadRequestException e)
            {
                return NotFound(e.Message);
            }

            return Ok(ConvertToAuctionDto(auctionBlo));
        }

        [HttpGet("[action]/{userId}/{count}/{skipCount}")]
        public async Task<ActionResult<List<AuctionDto>>> AllAuctionsWhichCreatUser(int userId, int count, int skipCount)
        {
            try
            {
                List<AuctionBlo> auctionBlos = await _auctionService.AllAuctionsWhichCreatUser(userId, count, skipCount);
                return Ok(ConvertToListAuctionDto(auctionBlos));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);

            }
        }

        [HttpGet("[action]/{userId}/{count}/{skipCount}")]
        public async Task<ActionResult<List<AuctionDto>>> AllAuctionsWhichMemberUser(int userId, int count, int skipCount)
        {
            try
            {
                List<AuctionBlo> auctionBlos = await _auctionService.AllAuctionsWhichMemberUser(userId, count, skipCount);
                return Ok(ConvertToListAuctionDto(auctionBlos));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);

            }
        }

        [HttpGet("[action]/{userId}/{count}/{skipCount}")]
        public async Task<ActionResult<List<AuctionDto>>> AllAvailableAuctions(int userId, int count, int skipCount)
        {
            try
            {
                List<AuctionBlo> auctionBlos = await _auctionService.AllAvailableAuctions(userId, count, skipCount);
                return Ok(ConvertToListAuctionDto(auctionBlos));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        private AuctionDto ConvertToAuctionDto(AuctionBlo auctionBlos)
        {
            if (auctionBlos == null)
                throw new ArgumentNullException(nameof(auctionBlos));

            AuctionDto auctionDtos = _mapper.Map<AuctionDto>(auctionBlos);

            return auctionDtos;
        }

        private List<AuctionDto> ConvertToListAuctionDto(List<AuctionBlo> auctionBlos)
        {
            if (auctionBlos == null)
                throw new ArgumentNullException(nameof(auctionBlos));

            List<AuctionDto> auctionDtos = new List<AuctionDto>();
            for (int i = 0; i < auctionBlos.Count; i++)
            {
                auctionDtos.Add(_mapper.Map<AuctionDto>(auctionBlos[i]));
            }

            return auctionDtos;
        }
    }
}