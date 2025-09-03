using Microsoft.AspNetCore.Mvc;
using Estoque.API.DTOs;
using Estoque.API.Services;

namespace Estoque.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (loginDto.Username == "admin" && loginDto.Password == "password")
            {
                var token = _tokenService.GenerateToken();
                return Ok(new { token });
            }

            return Unauthorized("Usuário ou senha inválidos.");
        }
    }
}
