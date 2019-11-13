using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IVMSBack.Models;
using IVMSBackApi.Models;
using DefaultData = IVMSBackApi.Models.DefaultData;
using PagedList;
using Newtonsoft.Json;

namespace IVMSBackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinesController : ControllerBase
    {
        private readonly IVMSBackContext _context;

        public LinesController(IVMSBackContext context)
        {
            _context = context;
        }

        // GET: api/Lines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Line>>> GetLine(int page, int start, int limit)
        {
            ResponseDefaultDataList response = new ResponseDefaultDataList();

            try
            {
                List<Filter> filtros;
                var filters = HttpContext.Request.Query["filter"].ToString();

                response.success = true;
                response.data = new List<Line>();
                List<Line> records = await _context.Line.Where(x => x.DateEnd == null).ToListAsync();

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

        // POST: api/Lines
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Line>> PostLine(Line line)
        {
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

        // DELETE: api/Lines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Line>> DeleteLine(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            Line line = _context.Line.Find(id);

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
