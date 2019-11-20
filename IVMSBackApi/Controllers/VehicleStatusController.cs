using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IVMSBack.Models;
using Microsoft.AspNetCore.Identity;
using IVMSBack.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using IVMSBackApi.Models;
using System.Security.Claims;
using Newtonsoft.Json;
using DefaultData = IVMSBackApi.Models.DefaultData;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;

namespace IVMSBackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Super Administrador, Administrador")]
    public class VehicleStatusController : ControllerBase
    {
        private readonly IVMSBackContext _context;
        private IWebHostEnvironment _environment;
        private readonly UserManager<IVMSBackUser> _userManager;
        private readonly RoleManager<IVMSBackRole> _roleManager;
        public IVMSBackUser CurrentUser { get; set; }
        public string CurrentUserId { get; set; }

        public VehicleStatusController(IVMSBackContext context,
            UserManager<IVMSBackUser> userManager,
            RoleManager<IVMSBackRole> roleManager,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _environment = environment;
        }

        // GET: api/VehicleStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleStatus>>> GetVehicleStatus(int page, int start, int limit)
        {
            ResponseDefaultDataList response = new ResponseDefaultDataList();

            try
            {
                List<Filter> filtros;
                var filters = HttpContext.Request.Query["filter"].ToString();

                response.success = true;
                response.data = new List<VehicleStatus>();
                List<VehicleStatus> records = await _context.VehicleStatus.Where(x => x.DateEnd == null).ToListAsync();
        
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

        // PUT: api/VehicleStatus/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize(Roles = "Super Administrador")]
        public async Task<IActionResult> PutVehicleStatus(int id, VehicleStatus vehicleStatus)
        {
            if (id != vehicleStatus.Id)
            {
                return BadRequest();
            }

            CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            vehicleStatus.UserModified =  CurrentUserId;
            vehicleStatus.DateModified = DateTime.Now;

            _context.Entry(vehicleStatus).State = EntityState.Modified;

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

        // POST: api/VehicleStatus
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize(Roles = "Super Administrador")]
        public async Task<ActionResult<VehicleStatus>> PostVehicleStatus(VehicleStatus vehicleStatus)
        {
            try
            {
                CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (!Directory.Exists(_environment.ContentRootPath + "/Upload"))
                {
                    Directory.CreateDirectory(_environment.ContentRootPath + "/Upload");
                }

                var file = Path.Combine(_environment.ContentRootPath, "Upload", "VehicleState" + DateTime.Now.Ticks + ".png");

                var base64Data = Regex.Match(vehicleStatus.Icon, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                var bytes = Convert.FromBase64String(base64Data);
                using (var imageFile = new FileStream(file, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                vehicleStatus.UserCreate =  CurrentUserId;
                vehicleStatus.DateCreate = DateTime.Now;
                
                _context.VehicleStatus.Add(vehicleStatus);

            
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

        // DELETE: api/VehicleStatus/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Super Administrador")]
        public async Task<ActionResult<VehicleStatus>> DeleteVehicleStatus(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            VehicleStatus vehicleStatus = _context.VehicleStatus.Find(id);

            vehicleStatus.UserEnd =  CurrentUserId;
            vehicleStatus.DateEnd = DateTime.Now;

            _context.Entry(vehicleStatus).State = EntityState.Modified;

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
