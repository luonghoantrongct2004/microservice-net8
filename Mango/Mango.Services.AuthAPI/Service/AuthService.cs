using Mango.Service.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManage;
        private readonly RoleManager<IdentityRole> _roleManage;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManage,
            RoleManager<IdentityRole> roleManage, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManage = userManage;
            _roleManage = roleManage;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManage.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManage.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManage.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u=>u.UserName.ToLower()==loginRequestDto.Username.ToLower());
            bool isValid = await _userManage.CheckPasswordAsync(user, loginRequestDto.Password);
            if (user == null || isValid==false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }
            var token = _jwtTokenGenerator.GenerateToken(user);
            UserDto userDto = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };
            return loginResponseDto;    
        }

        public async Task<string> Register(RegisterationRequestDto registerationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registerationRequestDto.Email,
                Email = registerationRequestDto.Email,
                NormalizedEmail = registerationRequestDto.Email.ToUpper(),
                Name = registerationRequestDto.Name,
                PhoneNumber = registerationRequestDto.PhoneNumber
            };
            try
            {
                var result = await _userManage.CreateAsync(user, registerationRequestDto.Password);
                if(result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registerationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Id = userToReturn.Id,
                        Email = userToReturn.Email,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }catch (Exception ex)
            {

            }
            return "Error Encoutered";
        }
    }
}
