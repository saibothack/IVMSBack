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

namespace IVMSBackApi.Controllers
{
    [Authorize(Roles = "Super Administrador, Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IVMSBackContext _context;

        public ProductsController(IVMSBackContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(int page, int start, int limit)
        {
            ResponseDefaultDataList response = new ResponseDefaultDataList();

            try
            {
                List<Filter> filtros;
                var filters = HttpContext.Request.Query["filter"].ToString();

                response.success = true;
                response.data = new List<Product>();
                List<Product> records = await _context.Product.Where(x => x.DateEnd == null).ToListAsync();

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

                            if (filtro.propiedad == "specifyLoad")
                            {
                                records = records.Where(x => x.SpecifyLoad.Equals(bool.Parse(filtro.valor))).ToList();
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

        // PUT: api/Products/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

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

        // POST: api/Products
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);

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

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            Product product = _context.Product.Find(id);

            product.DateEnd = DateTime.Now;

            _context.Entry(product).State = EntityState.Modified;

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
