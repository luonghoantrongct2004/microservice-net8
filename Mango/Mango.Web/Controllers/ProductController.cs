using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? list = new();
            ResponseDto? responseDto = await _productService.GetAllProductsAsync();
            if(responseDto!=null && responseDto.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }
            return View(list);
        }
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? responseDto = await _productService.CreateProductsAsync(productDto);
                if(responseDto!=null && responseDto.IsSuccess)
                {
                    TempData["success"] = $"Product {productDto.ProductName} created successful.";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message;
                }
            }
            return View(productDto);
        }
        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productId);
            if(responseDto!=null && responseDto.IsSuccess)
            {
                ProductDto? productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
                return View(productDto);
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? responseDto = await _productService.UpdateProductsAsync(productDto);
                if(responseDto!=null && responseDto.IsSuccess)
                {
                    TempData["success"] = $"Product {productDto.ProductName} updated successful.";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message;
                }
            }
            return View(productDto);
        }
        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productId);
            if(responseDto!=null && responseDto.IsSuccess)
            {
                ProductDto? productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto?.Result));
                return View(productDto);
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            ResponseDto? responseDto = await _productService.DeleteProductsAsync(productDto.ProductId);
            if(responseDto!=null && responseDto.IsSuccess )
            {
                TempData["success"] = responseDto?.Message;
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }
            return View(productDto);
        }
    }
}
