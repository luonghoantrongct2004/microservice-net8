using Mango.Web.Models;
using Mango.Web.Models.Dto;

namespace Mango.Web.Service.IService
{
    public interface IShoppingCartService
    {
        Task<ResponseDto?> ApplyCouponsAsync(CartDto CartDto);
        Task<ResponseDto?> GetCartByUserIdAsync(string userId);
        Task<ResponseDto?> UpsertCartsAsync(CartDto CartDto);
        Task<ResponseDto?> RemoveFromCartsAsync(int CartDetailsId);
    }
}
