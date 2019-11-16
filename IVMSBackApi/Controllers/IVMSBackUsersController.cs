using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVMSBack.Areas.Identity.Data;
using IVMSBack.Models;
using IVMSBackApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DefaultData = IVMSBackApi.Models.DefaultData;

namespace IVMSBackApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IVMSBackUsersController : ControllerBase
    {
        private readonly IVMSBackContext _context;
        private readonly UserManager<IVMSBackUser> _userManager;
        private readonly RoleManager<IVMSBackRole> _roleManager;

        public IVMSBackUsersController(IVMSBackContext context,
            UserManager<IVMSBackUser> userManager,
            RoleManager<IVMSBackRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/IVMSBackUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IVMSBackUser>>> GetAsync(int page, int start, int limit)
        {
            ResponseDefaultDataList response = new ResponseDefaultDataList();

            try
            {
                List<Filter> filtros;
                var filters = HttpContext.Request.Query["filter"].ToString();

                response.success = true;
                response.data = new List<IVMSBackUser>();
                List<IVMSBackUser> records = await _userManager.Users.Where(x => x.DateEnd == null).ToListAsync();

                if (!string.IsNullOrEmpty(filters))
                {
                    filtros = JsonConvert.DeserializeObject<List<Filter>>(filters);

                    foreach (var filtro in filtros)
                    {
                        if (!string.IsNullOrEmpty(filtro.valor))
                        {
                            if (filtro.propiedad == "name")
                            {
                                records = records.Where(x => x.Name.ToUpper().Contains(filtro.valor.ToUpper())).ToList();
                            }
                        }
                    }
                }

                response.total = records.Count();
                response.data.AddRange(records.Skip((page - 1) * limit).Take(limit));
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return BadRequest(response);
            }
        }

        [Authorize(Roles = "Super Administrador, Administrador")]
        // POST: api/IVMSBackUsers
        [HttpPost]
        public async Task<ActionResult<IVMSBackUser>> Post(IVMSBackUser iVMSBackUser)
        {
            try
            {
                if (IVMSBackUserExists(iVMSBackUser.Email))
                {
                    return BadRequest(new DefaultData
                    {
                        success = false,
                        message = "Su rol ya se encuentra dado de alta"
                    });
                }

                var user = new IVMSBackUser();
                user.Name = iVMSBackUser.Name;
                user.UserName = iVMSBackUser.Email;
                user.Email = iVMSBackUser.Email;
                //await _userManager.CreateAsync(user, iVMSBackUser.Password);
                var result = await _userManager.CreateAsync(user, "Sysware2016@");

                string errors = string.Empty;

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        errors += error.Description + ", ";
                    }
                }

                if (!string.IsNullOrEmpty(errors))
                {
                    return BadRequest(new DefaultData
                    {
                        success = false,
                        message = errors
                    });
                }

                var role = await _roleManager.FindByIdAsync(iVMSBackUser.RoleID);
                await _userManager.AddToRoleAsync(user, role.Name);
                

                return Ok(new DefaultData
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new DefaultData
                {
                    success = false,
                    message = ex.Message,
                });
            }
        }

        [Authorize(Roles = "Super Administrador, Administrador")]
        // PUT: api/IVMSBackUsers/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [Authorize(Roles = "Super Administrador, Administrador")]
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private bool IVMSBackUserExists(string email, string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                return _context.IVMSBackUser.Any(e => e.NormalizedEmail == email.ToUpper());
            }
            else
            {
                return _context.IVMSBackUser.Any(e => e.NormalizedEmail == email.ToUpper() && e.Id != id);
            }
        }
    }
}
