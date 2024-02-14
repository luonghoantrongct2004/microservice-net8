using Mango.Service.ShoppingCartApi.Models.Dtos;

namespace Mango.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProduct();

    }
}
