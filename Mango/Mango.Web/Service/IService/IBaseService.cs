﻿using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SeadAsync(RequestDto requestDto);
    }
}
