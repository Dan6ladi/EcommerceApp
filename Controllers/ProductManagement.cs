using ChoiceApp.Domain;
using Microsoft.AspNetCore.Mvc;
using ChoiceApp.SharedKernel.Models;
using Microsoft.AspNetCore.Authorization;
using ChoiceApp.ApplicationService.Interface;
using ChoiceApp.SharedKernel.Models.ProductModels;

namespace ChoiceApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductManagement : ControllerBase
    {
        private IProductService _productService;
        public ProductManagement(IProductService productService)
        {
            _productService = productService;
        }

        //Product Management – (Add, Edit, get List of Products) 

        /// <summary>
        /// Add New Product
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("addnewproduct")]
        public async Task<IActionResult> AddProduct([FromBody] AddProduct payload)
        {
            var response = await _productService.AddProduct(payload);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("updateproduct")]
        public async Task<IActionResult> UpdateProduct ([FromBody] AddProduct payload)
        {
            var response = await _productService.UpdateProduct(payload);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets List of Product
        /// </summary>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<List<Product>>))]
        [HttpGet]
        [Route("getlistofproducts")]
        public async Task<IActionResult> GetListOfProducts()
        {
            var response = await _productService.GetListOfProducts();

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("deleteproduct")]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            var response = await _productService.DeleteProduct(productId);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
