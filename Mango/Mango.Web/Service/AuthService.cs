﻿using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Mango.Web.Ultility;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto> AssignAsync(RegisterationRequestDto registerationRequestDto)
        {
            return await _baseService.SeadAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registerationRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/assignrole"
            });
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SeadAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/login"
            });
        }

        public async Task<ResponseDto> RegisterAsync(RegisterationRequestDto registerationRequestDto)
        {
            return await _baseService.SeadAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registerationRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/register"
            });
        }
    }
}
