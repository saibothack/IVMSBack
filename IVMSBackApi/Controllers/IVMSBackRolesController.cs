using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IVMSBack.Models;
using Microsoft.AspNetCore.Identity;
using IVMSBackApi.Models;
using Newtonsoft.Json;
using DefaultData = IVMSBackApi.Models.DefaultData;
using Microsoft.AspNetCore.Authorization;

namespace IVMSBackApi.Controllers
{
    [Authorize(Roles = "Super Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class IVMSBackRolesController : ControllerBase
    {
        private readonly IVMSBackContext _context;
        private readonly RoleManager<IVMSBackRole> _roleManager;

        public IVMSBackRolesController(IVMSBackContext context, 
            RoleManager<IVMSBackRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        // GET: api/IVMSBackRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IVMSBackRole>>> GetIVMSBackRole(int page, int start, int limit)
        {
            ResponseDefaultDataList response = new ResponseDefaultDataList();

            try
            {
                List<Filter> filtros;
                var filters = HttpContext.Request.Query["filter"].ToString();

                response.success = true;
                response.data = new List<IVMSBackRole>();
                List<IVMSBackRole> records = await _roleManager.Roles.Where(x => x.DateEnd == null).ToListAsync();

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

        [HttpGet]
        [Route("User")]
        public async Task<ActionResult<IEnumerable<IVMSBackRole>>> GetIVMSBackRoleUser()
        {
            ResponseDefaultDataList response = new ResponseDefaultDataList();

            try
            {
                response.success = true;
                response.data = new List<IVMSBackRole>();
                var noRole = await _roleManager.FindByNameAsync("Conductor");
                List<IVMSBackRole> records = await _roleManager.Roles.Where(x => x.DateEnd == null && x.Id != noRole.Id).ToListAsync();
                response.total = records.Count();
                response.data.AddRange(records);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return BadRequest(response);
            }
        }

        // PUT: api/IVMSBackRoles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIVMSBackRole(string id, IVMSBackRole iVMSBackRole)
        {
            if (id != iVMSBackRole.Id)
            {
                return BadRequest();
            }

            try
            {
                if (IVMSBackRoleExistsName(iVMSBackRole.Name, id))
                { 
                    return BadRequest(new DefaultData
                    {
                        success = false,
                        message = "Su rol ya se encuentra dado de alta"
                    });
                }

                IVMSBackRole role = await _roleManager.FindByIdAsync(id);
                role.Name = iVMSBackRole.Name;

                var result = await _roleManager.UpdateAsync(role);

                string errors = string.Empty;

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors) {
                        errors += error.Description +  ", ";
                    }
                }

                if (!string.IsNullOrEmpty(errors)) {
                    return BadRequest(new DefaultData
                    {
                        success = false,
                        message = errors
                    });
                }

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
                    message = ex.Message
                });
            }
        }

        // POST: api/IVMSBackRoles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<IVMSBackRole>> PostIVMSBackRole(IVMSBackRole iVMSBackRole)
        {

            try
            {
                if (IVMSBackRoleExistsName(iVMSBackRole.Name)) {
                    return BadRequest(new DefaultData
                    {
                        success = false,
                        message = "Su rol ya se encuentra dado de alta"
                    });
                }

                var role = new IVMSBackRole();
                role.Name = iVMSBackRole.Name;
                await _roleManager.CreateAsync(role);

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
                    message = ex.Message
                });
            }
        }

        // DELETE: api/IVMSBackRoles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IVMSBackRole>> DeleteIVMSBackRole(string id)
        {
            try
            {

                IVMSBackRole role = await _roleManager.FindByIdAsync(id);
                role.DateEnd = DateTime.Now;

                await _roleManager.UpdateAsync(role);

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
                    message = ex.Message
                });
            }
        }

        private bool IVMSBackRoleExistsName(string name, string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                return _context.IVMSBackRole.Any(e => e.NormalizedName == name.ToUpper());
            }
            else {
                return _context.IVMSBackRole.Any(e => e.NormalizedName == name.ToUpper() && e.Id != id);
            }
        }
    }
}
