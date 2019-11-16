using System;
using System.Security.Claims;
using System.Threading.Tasks;
using IVMSBack.Areas.Identity.Data;
using IVMSBack.Models;
using IVMSBackApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using DefaultData = IVMSBackApi.Models.DefaultData;
using System.Collections.Generic;
using System.Text;

namespace IVMSBackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IVMSBackUser> _userManager;
        private readonly RoleManager<IVMSBackRole> _roleManager;
        private readonly SignInManager<IVMSBackUser> _signInManager;
        public IConfiguration Configuration { get; }

        public AuthController(SignInManager<IVMSBackUser> signInManager, 
            UserManager<IVMSBackUser> userManager,
            RoleManager<IVMSBackRole> roleManager,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
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
                    var role = await _userManager.GetRolesAsync(user);
                    user.Role = await _roleManager.FindByNameAsync(role[0].ToString());
                    var token = GenerateTokenJwt(user);

                    return Ok(new LoginResponse { 
                        Token = token,
                        User = user,
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
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(Configuration["JWT:JwtExpireDays"]));

            var token = new JwtSecurityToken(
                Configuration["JWT:JwtIssuer"],
                Configuration["JWT:JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
