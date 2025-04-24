using Business;
using Business.Services;
using Entity.DTO.DTOLogin;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly RolUserService _rolUserResvice;
        private readonly JWTService _jwtService;



        public AuthController(AuthService authService, RolUserService rolUserResvice, JWTService jwtService)
        {
            _authService = authService;
            _rolUserResvice = rolUserResvice;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var user = await _authService.Login(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            var roles = await _rolUserResvice.GetRolesByUserId(user.Id);
            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, roles);

            return Ok(new { token });
        }
    }

}
