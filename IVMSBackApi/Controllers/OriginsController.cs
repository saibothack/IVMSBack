using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IVMSBack.Models;
using IVMSBackApi.Models;
using Newtonsoft.Json;
using DefaultData = IVMSBackApi.Models.DefaultData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IVMSBack.Areas.Identity.Data;
using System.Security.Claims;

namespace IVMSBackApi.Controllers
{
    [Authorize(Roles = "Super Administrador, Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class OriginsController : ControllerBase
    {
        private readonly IVMSBackContext _context;
        private readonly UserManager<IVMSBackUser> _userManager;
        private readonly RoleManager<IVMSBackRole> _roleManager;
        public IVMSBackUser CurrentUser { get; set; }
        public string CurrentUserId { get; set; }

        public OriginsController(IVMSBackContext context,
            UserManager<IVMSBackUser> userManager,
            RoleManager<IVMSBackRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

            CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        // GET: api/Origins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Origin>>> GetOrigin(int page, int start, int limit)
        {
            ResponseDefaultDataList response = new ResponseDefaultDataList();

            try
            {
                List<Filter> filtros;
                var filters = HttpContext.Request.Query["filter"].ToString();

                response.success = true;
                response.data = new List<Origin>();
                List<Origin> records = await _context.Origin.Where(x => x.DateEnd == null).ToListAsync();

                if (!string.IsNullOrEmpty(filters))
                {
                    filtros = JsonConvert.DeserializeObject<List<Filter>>(filters);

                    foreach (var filtro in filtros) {
                        if (!string.IsNullOrEmpty(filtro.valor))
                        {
                            if (filtro.propiedad == "name")
                            {
                                records = records.Where(x => x.Name.ToUpper().Contains(filtro.valor.ToUpper())).ToList();
                            } 

                            if (filtro.propiedad == "address")
                            {
                                records = records.Where(x => x.Address.ToUpper().Contains(filtro.valor.ToUpper())).ToList();
                            } 
                        }
                    }
                }

                response.total = records.Count();
                response.data.AddRange(records.Skip((page - 1) * limit).Take(limit));
                return Ok(response);
            }
            catch (Exception ex) {
                response.success = false;
                response.message = ex.Message;
                return BadRequest(response);
            }
        }

        [Authorize(Roles = "Super Administrador")]
        // PUT: api/Origins/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrigin(int id, Origin origin)
        {
            if (id != origin.Id)
            {
                return BadRequest();
            }

            origin.UserModified =  CurrentUserId;
            origin.DateModified = DateTime.Now;

            _context.Entry(origin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

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

        [Authorize(Roles = "Super Administrador")]
        // POST: api/Origins
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Origin>> PostOrigin(Origin origin)
        {

            origin.UserCreate =  CurrentUserId;
            origin.DateCreate = DateTime.Now;

            _context.Origin.Add(origin);

            try
            {
                await _context.SaveChangesAsync();

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

        [Authorize(Roles = "Super Administrador")]
        // DELETE: api/Origins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Origin>> DeleteOrigin(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            Origin origin = _context.Origin.Find(id);

            origin.UserEnd =  CurrentUserId;
            origin.DateEnd = DateTime.Now;
            

            _context.Entry(origin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

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
    }
}
