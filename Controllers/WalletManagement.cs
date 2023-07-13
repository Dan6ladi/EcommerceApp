using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using ChoiceApp.ApplicationService.Interface;
using ChoiceApp.SharedKernel.Models.ProductModels;
using ChoiceApp.SharedKernel.Models;
using System.Security.Claims;

namespace ChoiceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletManagement : ControllerBase
    {
        //Fund with debit card, Withdraw from wallet)

        private IWalletService _walletService;
        public WalletManagement(IWalletService walletService)
        {
            _walletService = walletService;
        }

        /// <summary>
        /// Fund Wallet With Debit Card
        /// </summary>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("fundwallet")]
        public async Task<IActionResult> FundWallet(string sourceAccountNo, decimal amount)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (email == null)
            {
                return Unauthorized("Unauthorized request, kindly log in");
            }
            var response = await _walletService.FundWalletWithCard(sourceAccountNo,amount,email.Value);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// withdraw from wallet
        /// </summary>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("withdrawfromwallet")]
        public async Task<IActionResult> Withdraw(string sourceAccountNo, decimal amount)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (email == null)
            {
                return Unauthorized("Unauthorized request, kindly log in");
            }
            var response = await _walletService.WithdrawFromWallet(sourceAccountNo, amount, email.Value);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
