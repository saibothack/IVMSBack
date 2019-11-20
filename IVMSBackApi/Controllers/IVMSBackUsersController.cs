using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public IVMSBackUser CurrentUser { get; set; }
        public string CurrentUserId { get; set; }

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
                CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                CurrentUser = await _userManager.FindByIdAsync(userId);
                var CurrentUserRole = await _userManager.GetRolesAsync(CurrentUser);


                List<Filter> filtros;
                var filters = HttpContext.Request.Query["filter"].ToString();

                response.success = true;
                response.data = new List<IVMSBackUser>();
                
                List<IVMSBackUser> records = new List<IVMSBackUser>();
                var roles = await _roleManager.Roles.Where(x => x.Name != "Conductor" && x.DateEnd == null).ToListAsync();
                foreach(var role in roles) {
                    List<IVMSBackUser> users = new List<IVMSBackUser>();

                    if (CurrentUserRole[0] == "Super Administrador") {
                        users = ((List<IVMSBackUser>) await _userManager.GetUsersInRoleAsync(role.Name)).Where(x => x.DateEnd == null).ToList();
                    } else {
                        var lines = await _context.IVMSBackUserLines
                                    .Include(x => x.Line)
                                    .Where(x => x.IVMSBackUserID.Contains(userId) && x.DateEnd == null && x.Line.DateEnd == null)
                                    .Select(x => x.Line).ToListAsync();

                        var usersFilter = await _context.IVMSBackUserLines
                                    .Include(x => x.IVMSBackUser)
                                    .Include(x => x.Line)
                                    .Where(x => x.DateEnd == null && lines.Contains(x.Line) && x.IVMSBackUser.DateEnd == null)
                                    .Select(x => x.IVMSBackUser).ToListAsync();
                                
                        if (lines.Count > 0) users = ((List<IVMSBackUser>)await _userManager.GetUsersInRoleAsync(role.Name))
                                                                            .Where(x => x.DateEnd == null && usersFilter.Contains(x))
                                                                            .ToList();
                    }

                    records.AddRange(users);
                }
                
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

                CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

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
