using CouponApi.Application.DTO;
using CouponApi.Application.Interfaces;
using CouponApi.Presentation.Domain.Entities;
using ECommercelib.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CouponApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoCodeController : ControllerBase
    {
        private readonly IPromoCode _promoCodeRepository;

        public PromoCodeController(IPromoCode promoCodeRepository)
        {
            _promoCodeRepository = promoCodeRepository;
        }

        [HttpGet("{userId}/available")]
        public async Task<ActionResult<IEnumerable<PromoCode>>> GetAvailablePromoCodesWithDetails()
        {
            var promoCodes = await _promoCodeRepository.GetAvailablePromoCodes(0); 
            return Ok(promoCodes);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<PromoCode>>> GetUserPromoCodes(int userId)
        {
            var promoCodes = await _promoCodeRepository.GetUserPromoCodes(userId);


            if (promoCodes == null || !promoCodes.Any())
            {
                return NotFound("Промокоды не найдены.");
            }

            return Ok(promoCodes);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Response>> AddPromoCode([FromBody] PromoCodeDTO promoCodeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await  _promoCodeRepository.AddPromoCode(promoCodeDTO);

            if (result.Flag)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("redeem")]
        public async Task<ActionResult<Response>> RedeemPromoCode([FromBody] RedeemPromoCodeRequest request)
        {
            if (request.BonusAmount <= 0)
            {
                return BadRequest("У вас должны быть бонусы ");
            }

            var response = await _promoCodeRepository.RedeemPromoCode(request.PromoCodeId, request.UserId);
            return Ok(response);
        }
    }
}

