using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRegisterDynamo.Models;
using UserRegisterDynamo.Repository;

namespace UserRegisterDynamo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController:ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository, TokenService tokenService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _tokenService = tokenService;   
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateUser user)
        {
            try
            {
                await _userRepository.Add(user);
                var token = _tokenService.GenerateToken(user.Username);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetByCpf(string cpf,string username,string password)
        {
            var user = await _userRepository.GetByCpf(cpf, username, password);
            if (user == null) {
                return BadRequest("Dados inválidos!");
            }
            return Ok(user);
        }
        [HttpPost("ConfirmationCode")]
        public async Task<IActionResult> ConfirmationCode(string cpf, string username, string password,string code)
        {
            bool confirm = await _userRepository.ConfirmCode(cpf, username, password,code);
            if (confirm)
            {
                return Ok("Confirmado com sucesso!");
            }
            return BadRequest("Error ao confirmar o usuário!");
        }
    }
}
