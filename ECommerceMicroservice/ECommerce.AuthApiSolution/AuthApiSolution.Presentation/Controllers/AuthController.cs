using AuthApiSolution.Application.DTOs;
using AuthApiSolution.Application.Interfaces;
using ECommercelib.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApiSolution.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController(IUser userInterface) : ControllerBase
    {

        [HttpPost("Register")]
        public async Task<ActionResult<Response>> Register(AppUserDTO appUserDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await userInterface.Register(appUserDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Response>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await userInterface.Login(loginDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> GetUser (int id)
        {
            if(id <= 0) return BadRequest("Неверный Id пользователя");
            var user = await userInterface.GetUser(id);
            return user.Id > 0 ? Ok(user) : NotFound();
        }
    }
}
