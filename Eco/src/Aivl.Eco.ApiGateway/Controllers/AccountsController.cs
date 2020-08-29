namespace Eco.ApiGateway.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Eco.Sql.Models;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IConfiguration configuration;

        private readonly IMapper mapper;

        private readonly SignInManager<UserEntity> signInManager;

        private readonly UserManager<UserEntity> userManager;

        private readonly EmailManager emailManager;

        public AccountsController(
            IConfiguration configuration,
            IMapper mapper,
            UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager,
            EmailManager emailManager)
        {
            this.configuration = configuration;

            this.mapper = mapper;

            this.signInManager = signInManager;

            this.userManager = userManager;

            this.emailManager = emailManager;
        }

        [HttpGet]
        [Authorize]
        [Route("my")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var user = await this.userManager.FindByNameAsync(this.CurrentUserInfo.UserName);

            if (user != null)
            {
                var startupUser = this.mapper.Map<User>(user);

                return Ok(startupUser);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var passwordSignResult = await this.signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

            if (passwordSignResult.IsLockedOut)
            {
                return Unauthorized("Locked Out");
            }
            else if (passwordSignResult.Succeeded)
            {
                var user = await this.userManager.FindByNameAsync(request.UserName);

                var roles = await this.userManager.GetRolesAsync(user);

                var token = await this.GenerateJwtToken(user, roles);

                return Ok(new LoginResponse() { Token = token, Roles = roles });
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(CreateAccountResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
           return Ok(await Create(request));
        }

        [HttpPut]
        [Authorize]
        [Route("update")]
        [ProducesResponseType(typeof(UpdateAccountResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountRequest request)
        {
            var user = await this.userManager.FindByNameAsync(this.CurrentUserInfo.UserName);

            if (user != null)
            {
                user.UserName = request.UserName;

                user.Email = request.Email;

                user.ProfileLink = request.ProfileLink;

                user.FullName = request.FullName;

                user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;

                user.ModifiedDate = DateTimeOffset.UtcNow;

                var updateResult = await this.userManager.UpdateAsync(user);

                return Ok(new UpdateAccountResponse()
                {
                    Result = updateResult.Succeeded,
                    ErrorCodes = updateResult.Errors.Select(e => e.Code)
                });
            }

            return BadRequest();
        }

        [HttpPut]
        [Authorize]
        [Route("changepassword")]
        [ProducesResponseType(typeof(Response<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordRequest request)
        {
            var identityUser = await this.userManager.FindByEmailAsync(this.CurrentUserInfo.Email);

            if (identityUser != null)
            {
                var result = await this.userManager.ChangePasswordAsync(identityUser, request.OldPassword, request.NewPassword);

                if (result.Errors != null && result.Errors.Count() > 0)
                {
                    return Ok(new Response<bool>()
                    {
                        ErrorCodes = result.Errors.Select(e => e.Description),
                        Result = false,
                    });
                }

                return Ok(new Response<bool>()
                {
                    ErrorCodes = null,
                    Result = true,
                });
            }

            return Ok(new Response<bool>()
            {
                ErrorCodes = new List<string>() { "User Not Exist" },
                Result = false,
            });
        }

        [HttpPut]
        [Route("forgotpassword/{email}")]
        [ProducesResponseType(typeof(Response<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ForgotPassword([FromRoute] string email)
        {
            var identityUser = await this.userManager.FindByEmailAsync(email);

            if (identityUser != null)
            {
                var token = await this.userManager.GeneratePasswordResetTokenAsync(identityUser);

                var result = await this.emailManager.SendEmail(new Email()
                {
                    Subject = "",
                    Body = "",
                    To = email,
                    BodyIsHtml = false,
                    Url = this.configuration["RestPasswordUrl"] + "?Token=" + token
                });

                return Ok(new Response<bool>()
                {
                    ErrorCodes = null,
                    Result = result
                });
            }

            return Ok(new Response<bool>()
            {
                ErrorCodes = new List<string>() { "User Not Exist" },
                Result = false,
            });
        }

        [HttpPut]
        [Route("resetpassword")]
        [ProducesResponseType(typeof(Response<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ResetPassword([FromBody] ForgotPasswordRequest request)
        {
            var identityUser = await this.userManager.FindByEmailAsync(request.Email);

            if (identityUser != null)
            {

                var result = await this.userManager.ResetPasswordAsync(identityUser, request.Token, request.Password);

                if (result.Errors != null && result.Errors.Count() > 0)
                {
                    return Ok(new Response<bool>()
                    {
                        ErrorCodes = result.Errors.Select(e => e.Description),
                        Result = false
                    });
                }

                return Ok(new Response<bool>()
                {
                    ErrorCodes = null,
                    Result = true
                });
            }

            return Ok(new Response<bool>()
            {
                ErrorCodes = new List<string>() { "User Not Exist" },
                Result = false,
            });
        }

        [Authorize]
        [HttpGet]
        [Route("refreshtoken")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> RefreshToken()
        {
            var user = await this.userManager.FindByNameAsync(CurrentUserInfo.UserName);

            var roles = await this.userManager.GetRolesAsync(user);

            var token = await this.GenerateJwtToken(user, roles);

            return Ok(new LoginResponse() { Token = token, Roles = roles });
        }

        private async Task<object> GenerateJwtToken(UserEntity user, IList<string> roles)
        {
            var userClaims = await this.userManager.GetClaimsAsync(user);

            userClaims.Add(new Claim(ClaimsKeys.Roles, string.Join(',', roles)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JwtKey"]));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddDays(Convert.ToDouble(this.configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                this.configuration["JwtIssuer"],
                this.configuration["JwtAudience"],
                userClaims,
                expires: expires,
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<CreateAccountResponse> Create(CreateAccountRequest request)
        {
            var result = false;
            var createResult = await this.userManager.CreateAsync(new UserEntity()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                ProfileLink = request.ProfileLink,
                FullName = request.FullName
            }, request.Password);

            var errorCodes = createResult.Errors.Select(e => e.Code);

            if (createResult.Succeeded)
            {
                var createdUser = await this.userManager.FindByNameAsync(request.UserName);

                var claims = new List<Claim>()
                {
                    new Claim(ClaimsKeys.UserName, request.UserName),
                    new Claim(ClaimsKeys.Email, request.Email)
                };

                await this.userManager.AddClaimsAsync(createdUser, claims);

                var addToRoleResult = await this.userManager.AddToRoleAsync(createdUser, Constants.User);

                result = addToRoleResult.Succeeded;

                errorCodes = errorCodes.Concat(addToRoleResult.Errors.Select(e => e.Code));
            }

            return new CreateAccountResponse()
            {
                Result = result,
                ErrorCodes = errorCodes
            };
        }
    }
}
