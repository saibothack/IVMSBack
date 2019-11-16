using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IVMSBack.Models;
using IVMSBackApi.Models;
using DefaultData = IVMSBackApi.Models.DefaultData;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using IVMSBack.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace IVMSBackApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LinesController : ControllerBase
    {
        private readonly IVMSBackContext _context;
        private readonly UserManager<IVMSBackUser> _userManager;
        private readonly RoleManager<IVMSBackRole> _roleManager;
        public IVMSBackUser CurrentUser { get; set; }
        public string CurrentUserId { get; set; }

        public  LinesController(IVMSBackContext context,
            UserManager<IVMSBackUser> userManager,
            RoleManager<IVMSBackRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

            CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        // GET: api/Lines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Line>>> GetLine(int page, int start, int limit)
        {
            ResponseDefaultDataList response = new ResponseDefaultDataList();

            try
            {
                CurrentUser = await _userManager.FindByIdAsync(CurrentUserId);
                var role = await _userManager.GetRolesAsync(CurrentUser);

                List<Filter> filtros;
                var filters = HttpContext.Request.Query["filter"].ToString();

                response.success = true;
                response.data = new List<Line>();
                List<Line> records = new List<Line>();

                if (role[0] == "Super Administrador") {
                    records = await _context.Line.Where(x => x.DateEnd == null).ToListAsync();
                } else {
                    var lines = (from a in (await _context.IVMSBackUserLines
                                .Include(x => x.Line)
                                .Where(x => x.IVMSBackUserID.Equals(CurrentUserId) && x.DateEnd == null && x.Line.DateEnd == null)
                                .ToListAsync())
                                select a.Line).ToList();

                    if (lines.Count > 0) records = await _context.Line.Where(x => x.DateEnd == null && lines.Contains(x)).ToListAsync();
                }

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

        [Authorize(Roles = "Super Administrador, Administrador")]
        // PUT: api/Lines/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLine(int id, Line line)
        {
            if (id != line.Id)
            {
                return BadRequest();
            }

            line.UserModified =  CurrentUserId;
            line.DateModified = DateTime.Now;

            _context.Entry(line).State = EntityState.Modified;

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

        [Authorize(Roles = "Super Administrador, Administrador")]
        // POST: api/Lines
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Line>> PostLine(Line line)
        {
            line.UserCreate =  CurrentUserId;
            line.DateCreate = DateTime.Now;
            
            _context.Line.Add(line);

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

        [Authorize(Roles = "Super Administrador, Administrador")]
        // DELETE: api/Lines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Line>> DeleteLine(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            Line line = _context.Line.Find(id);

            line.UserEnd =  CurrentUserId;
            line.DateEnd = DateTime.Now;

            _context.Entry(line).State = EntityState.Modified;

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
