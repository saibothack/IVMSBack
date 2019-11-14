using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IVMSBack.Areas.Identity.Data;
using IVMSBackApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using DefaultData = IVMSBackApi.Models.DefaultData;

namespace IVMSBackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IVMSBackUser> _userManager;
        private readonly SignInManager<IVMSBackUser> _signInManager;
        public IConfiguration Configuration { get; }

        public AuthController(SignInManager<IVMSBackUser> signInManager, 
            UserManager<IVMSBackUser> userManager,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            Configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<DefaultData>> Post(IVMSBackUser login)
        {
            try
            {
                if (login == null) {
                    return BadRequest(new DefaultData
                    {
                        success = false,
                        message = "Por favor ingrese su email y contraseña"
                    });
                }

                var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(login.UserName);
                    var token = GenerateTokenJwt(user);

                    return Ok(new LoginResponse { 
                        Token = token,
                        success = true
                    });
                }
                else {
                    return Unauthorized(new LoginResponse
                    {
                        success = false,
                        message = "Por favor verifique su usuario y password"
                    });
                }
            }
            catch (Exception ex) {
                return Unauthorized(new LoginResponse
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        private string GenerateTokenJwt(IVMSBackUser user)
        {
            user.SecurityStamp = null;
            user.PasswordHash = null;
            user.ConcurrencyStamp = null;
            user.PhoneNumber = null;

            // appsetting for Token JWT
            var secretKey = Configuration["JWT:ClaveSecreta"];
            var audienceToken = Configuration["JWT:Audience"];
            var issuerToken = Configuration["JWT:Issuer"];

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create a claimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, JsonConvert.SerializeObject(user))
                });

            // create token to the user
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }
    }
}
