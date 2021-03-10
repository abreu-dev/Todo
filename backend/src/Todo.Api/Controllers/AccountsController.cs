using Todo.Application.DTOs.AccountDTO;
using Todo.Application.DTOs.UserDTOs;
using Todo.Application.Extensions;
using Todo.Application.Interfaces;
using Todo.Domain.Entities;
using Todo.Domain.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Api.Controllers
{
    public class AccountsController : ApiController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppJwtSettings _appJwtSettings;
        private readonly IEmailSenderService _emailSenderService;

        public AccountsController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IOptions<AppJwtSettings> appJwtSettings, 
            IEmailSenderService emailSenderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appJwtSettings = appJwtSettings.Value;
            _emailSenderService = emailSenderService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new ApplicationUser
            {
                UserName = registerUserDTO.Username,
                Email = registerUserDTO.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUserDTO.Password);

            if (result.Succeeded)
            {
                return CustomResponse(await GetFullJwt(user.UserName));
            }

            foreach (var error in result.Errors)
            {
                AddError(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUserDTO.Username, loginUserDTO.Password, false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await GetFullJwt(loginUserDTO.Username));
            }

            if (result.IsLockedOut)
            {
                AddError("This user is temporarily blocked.");
                return CustomResponse();
            }

            AddError("Incorrect username or password.");
            return CustomResponse();
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotUserPasswordDTO forgotUserPasswordDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _userManager.FindByNameAsync(forgotUserPasswordDTO.Username);

            if (user == null)
            {
                AddError("This user was not found.");
                return CustomResponse();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            if (await _emailSenderService.SendForgotPasswordEmail(user.Email, user.UserName, token))
            {
                return CustomResponse();
            }

            AddError("Email cannot be sent.");
            return CustomResponse();
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetUserPasswordDTO resetUserPasswordDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _userManager.FindByNameAsync(resetUserPasswordDTO.Username);

            var result = await _userManager.ResetPasswordAsync(user, resetUserPasswordDTO.Token, resetUserPasswordDTO.NewPassword);

            if (result.Succeeded)
            {
                if (await _emailSenderService.SendResetedPasswordEmail(user.Email, user.UserName))
                {
                    return CustomResponse();
                }

                AddError("Email cannot be sent.");
                return CustomResponse();
            }

            foreach (var error in result.Errors)
            {
                AddError(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost]
        [Route("forgot-username")]
        public async Task<IActionResult> ForgotUserName([FromBody] ForgotUserNameDTO forgotUserNameDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _userManager.FindByEmailAsync(forgotUserNameDTO.Email);

            if (user == null)
            {
                AddError("This user was not found.");
                return CustomResponse();
            }

            if (await _emailSenderService.SendForgotUserNameEmail(user.Email, user.UserName)) 
            {
                return CustomResponse();
            }

            AddError("Email cannot be sent.");
            return CustomResponse();
        }

        private async Task<LoginResponseDTO> GetFullJwt(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var claims = await _userManager.GetClaimsAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appJwtSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appJwtSettings.Issuer,
                Audience = _appJwtSettings.Audience,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appJwtSettings.Expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return new LoginResponseDTO
            {
                AccessToken = encodedToken,
                Username = user.UserName,
                Email = user.Email
            };
        }
    }
}
