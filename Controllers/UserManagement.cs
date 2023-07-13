using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ChoiceApp.SharedKernel.Models;
using ChoiceApp.ApplicationService.Interface;
using ChoiceApp.SharedKernel.Models.UserModels;
using ChoiceApp.Infrastructure.Manager.Authentication;

namespace ChoiceApp.Controllers
{
    /// <summary>
    /// Handles User Operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagement : ControllerBase
    {
        private readonly IUserService _userService;

        public UserManagement(IUserService userService)
        {
            _userService = userService;
        }


        //User Management - Login, Register, Forgot Password, Pin Reset
        /// <summary>
        /// Register as a New User on the PlatForm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("register")]
        [HttpPost]
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var registerResponse = await _userService.Register(request);
            if (registerResponse.HasError)
            {
                return BadRequest(registerResponse);
            }
            return Ok(registerResponse);

        }


        /// <summary>
        /// Login on the Platform
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        [Produces(typeof(GeneralResponseWrapper<LoginResponse>))]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var loginResponse = await _userService.Login(request);
            if (loginResponse.HasError)
            {
                return BadRequest(loginResponse);
            }
            return Ok(loginResponse);
        }


        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Route("forgotpassword")]
        [HttpPost]
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var response = await _userService.SendCodeToEmail(email);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }


        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name=" request"></param>
        /// <returns></returns>
        [Route("resetpassword")]
        [HttpPost]
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (email == null)
            {
                return Unauthorized("Unauthorized request, kindly log in");
            }
            var response = await _userService.ChangePassword(request, email.Value);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }


        /// <summary>
        /// Verify code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("verifycode")]
        [HttpPost]
        [Produces(typeof(GeneralResponseWrapper<LoginResponse>))]
        public async Task<IActionResult> VerifyCode(string code)
        {
            var response = await _userService.ValidateCode(code);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
