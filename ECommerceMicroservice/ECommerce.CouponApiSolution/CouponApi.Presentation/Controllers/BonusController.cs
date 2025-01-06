using CouponApi.Application.DTO;
using CouponApi.Application.Interfaces;
using ECommercelib.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonusController : ControllerBase
    {
        private readonly IBonusRepository _bonusService;

        public BonusController(IBonusRepository bonusService)
        {
            _bonusService = bonusService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> AddBonus([FromBody] BonusDTO bonusDTO)
        {
            bonusDTO = new BonusDTO(
                bonusDTO.Id,            
                bonusDTO.UserId,               
                bonusDTO.Amount,
                bonusDTO.EarnedDate,
                false                
            );

            var response = await _bonusService.AddBonus(bonusDTO);
            return Ok(response);
        }

        [HttpGet("{userId}/total")]
        public async Task<ActionResult<int>> GetUserTotalBonus(int userId)
        {
            var totalBonus = await _bonusService.GetUserTotalBonus(userId);
            return Ok(totalBonus);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<BonusDTO>>> GetBonusesByUserId(int userId)
        {
            var bonuses = await _bonusService.GetBonusesByUserId(userId);
            return Ok(bonuses);
        }

        [HttpPost("bottledrop")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBottleDeposit(int userId, [FromBody] BottleDepositDTO bottleDepositDTO)
        {
            var response = await _bonusService.ReturnBottle(userId, bottleDepositDTO);
            return Ok(response);
        }

        [HttpGet("bottledrops/{userId}")]
        public async Task<IActionResult> GetBottleDeposits(int userId)
        {
            var deposits = await _bonusService.GetUserBottleDeposits(userId);
            return Ok(deposits);
        }
    }
}
