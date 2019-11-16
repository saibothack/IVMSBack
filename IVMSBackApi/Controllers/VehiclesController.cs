using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IVMSBack.Models;
using Microsoft.AspNetCore.Authorization;
using IVMSBackApi.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using IVMSBack.Areas.Identity.Data;
using DefaultData = IVMSBackApi.Models.DefaultData;

namespace IVMSBackApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVMSBackContext _context;
        private readonly UserManager<IVMSBackUser> _userManager;
        private readonly RoleManager<IVMSBackRole> _roleManager;
        public IVMSBackUser CurrentUser { get; set; }
        public string CurrentUserId { get; set; }

        public VehiclesController(IVMSBackContext context,
            UserManager<IVMSBackUser> userManager,
            RoleManager<IVMSBackRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

            CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        // GET: api/Vehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicle(int page, int start, int limit)
        {
            
           ResponseDefaultDataList response = new ResponseDefaultDataList();

            try
            {
                CurrentUser = await _userManager.FindByIdAsync(CurrentUserId);
                var CurrentUserRole = await _userManager.GetRolesAsync(CurrentUser);

                List<Filter> filtros;
                var filters = HttpContext.Request.Query["filter"].ToString();

                response.success = true;
                response.data = new List<Vehicle>();

                List<Vehicle> records = new List<Vehicle>();

                if (CurrentUserRole[0] == "Super Administrador") {
                    records = await _context.Vehicle.Where(x => x.DateEnd == null).ToListAsync();
                } else {
                    var lines = await _context.IVMSBackUserLines
                                .Include(x => x.Line)
                                .Where(x => x.IVMSBackUserID.Contains(CurrentUserId) && x.DateEnd == null && x.Line.DateEnd == null)
                                .Select(x => x.Line).ToListAsync();

                    var vehicles = await _context.VehicleLines
                                .Include(x => x.Vehicle)
                                .Include(x => x.Line)
                                .Where(x => x.DateEnd == null && lines.Contains(x.Line) && x.Vehicle.DateEnd == null && x.Line.DateEnd == null)
                                .Select(x => x.Vehicle).ToListAsync();
                            
                    if (lines.Count > 0) records = await _context.Vehicle.Where(x => x.DateEnd == null && vehicles.Contains(x)).ToListAsync();
                }

                foreach(var vehicle in records) {
                    vehicle.Line = _context.VehicleLines.Where(x => x.DateEnd == null && x.VehicleID.Equals(vehicle.Id))
                                    .Include(x => x.Line)
                                    .FirstOrDefault().Line.Name;

                    vehicle.VehicleStatus = _context.VehicleStatusStore.Where(x => x.DateEnd == null && x.VehicleID.Equals(vehicle.Id))
                                    .Include(x => x.VehicleStatus)
                                    .FirstOrDefault().VehicleStatus.Name;

                    vehicle.User = _context.IVMSBackUserVehicles.Where(x => x.DateEnd == null && x.VehicleID.Equals(vehicle.Id))
                                    .Include(x => x.IVMSBackUser)
                                    .FirstOrDefault().IVMSBackUser.Name;
                    
                }

                if (!string.IsNullOrEmpty(filters))
                {
                    filtros = JsonConvert.DeserializeObject<List<Filter>>(filters);

                    foreach (var filtro in filtros) {
                        if (!string.IsNullOrEmpty(filtro.valor))
                        {
                            /*if (filtro.propiedad == "name")
                            {
                                records = records.Where(x => x.Name.ToUpper().Contains(filtro.valor.ToUpper())).ToList();
                            } 

                            if (filtro.propiedad == "address")
                            {
                                records = records.Where(x => x.Address.ToUpper().Contains(filtro.valor.ToUpper())).ToList();
                            } */
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
        // PUT: api/Vehicles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(int id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest();
            }

            if (VehicleExists(vehicle, id))
            {
                return BadRequest(new DefaultData
                {
                    success = false,
                    message = "Sus placas ya fueron utilizadas"
                });
            }

            vehicle.UserModified =  CurrentUserId;
            vehicle.DateModified = DateTime.Now;

            _context.Entry(vehicle).State = EntityState.Modified;

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
        // POST: api/Vehicles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
            if (VehicleExists(vehicle))
            {
                return BadRequest(new DefaultData
                {
                    success = false,
                    message = "Sus placas ya fueron utilizadas"
                });
            }


            vehicle.UserCreate =  CurrentUserId;
            vehicle.DateCreate = DateTime.Now;
            _context.Vehicle.Add(vehicle);

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
        // DELETE: api/Vehicles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vehicle>> DeleteVehicle(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            Vehicle vehicle = _context.Vehicle.Find(id);

            vehicle.UserModified =  CurrentUserId;
            vehicle.DateModified = DateTime.Now;

            _context.Entry(vehicle).State = EntityState.Modified;

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

        private bool VehicleExists(Vehicle vehicle, int id = 0)
        {
            if (id == 0){
                return _context.Vehicle.Any(e => e.Plates.ToUpper().Contains(vehicle.Plates.ToUpper()));
            } else {
                return _context.Vehicle.Any(e => e.Plates.ToUpper().Contains(vehicle.Plates.ToUpper()) && !e.Id.Equals(id));
            }
        }
    }
}
