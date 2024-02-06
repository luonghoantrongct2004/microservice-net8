using Mango.Service.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = errorMessage;
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _authService.Login(loginRequestDto);
            if(loginResponse.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username and password is incorrect.";
                return BadRequest(_responseDto);
            }
            _responseDto.Result = loginResponse;
            return Ok(_responseDto);
        }
        [HttpPost("assignrole")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterationRequestDto model)
        {
            var assignRoleSuccessFul = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessFul)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error encountered.";
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
    }
}
