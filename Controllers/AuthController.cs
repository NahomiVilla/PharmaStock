using Microsoft.AspNetCore.Mvc;
using PharmaStock.Models;
using PharmaStock.Repositories;
using PharmaStock.Services;
using BCrypt.Net;

namespace PharmaStock.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserRepository _userRepository;
        private readonly AuthService _authService;

        public AuthController(UserRepository userRepository, AuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult  Login([FromBody] DTOLogin model)
        {
            var user = _userRepository.GetByEmail(model.Email);

            if (user == null|| !PasswordService.VerifyPassword(model.Password, user.Password)){
                return Unauthorized(new { message = "Credenciales Incorrectas" });
            }
            var token =_authService.GenerateJWTToken(user);
            return Ok(new{message="login exitoso",token});
               
                
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] Users model)
        {
            var user = _userRepository.GetByEmail(model.Email);

            if (user != null)
                return NotFound(new { message = "Usuário já cadastrado" });

            model.Password = PasswordService.HashPassword(model.Password);
            var nuevoUsuario = _userRepository.Registrar(model);
            return Ok(nuevoUsuario);
        }
    }
}