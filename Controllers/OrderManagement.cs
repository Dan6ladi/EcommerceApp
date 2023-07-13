using System.Security.Claims;
using ChoiceApp.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using ChoiceApp.SharedKernel.Models;
using ChoiceApp.ApplicationService.Interface;
using ChoiceApp.ApplicationService.Services;
using ChoiceApp.SharedKernel.Models.ProductModels;
using ChoiceApp.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ChoiceApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderManagement : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrderManagement(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //Order Management – (PLACE ORDER, Return, Refund and User Cart and Order History)

        /// <summary>
        /// Customer places order
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("placeorder")]
        public async Task<IActionResult> PlaceOrder(OrderDto payload)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (email == null)
            {
                return Unauthorized("Unauthorized request, kindly log in");
            }
            var response = await _orderService.PlaceOrder(payload, email.Value);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Customer adds products to be ordered to cart
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("addtocart")]
        public async Task<IActionResult> AddToCart(CartDto payload)
        {
            var response = await _orderService.AddToCart(payload);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets customer's order history
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpGet]
        [Route("getorderhistory")]
        public async Task<IActionResult> OrderHistory()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (email == null)
            {
                return Unauthorized("Unauthorized request, kindly log in");
            }
            var response = await _orderService.GetOrderHistory(email.Value);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
