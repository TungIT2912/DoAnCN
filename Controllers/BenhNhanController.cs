using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace WebQuanLyNhaKhoa.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BenhNhanController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BenhNhanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BenhNhan
        // Only accessible by Admin role
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BenhNhan>>> GetBenhNhans(string query = "", string filter = "nothing")
        {
            var currentDay = DateTime.Now;
            IQueryable<BenhNhan> queryable = _context.BenhNhans;

            if (!string.IsNullOrEmpty(query))
            {
                queryable = queryable.Where(n => n.HoTen.Contains(query));
            }

            switch (filter)
            {
                case "Nam":
                    queryable = queryable.Where(n => n.Gioi == false);
                    break;
                case "Nữ":
                    queryable = queryable.Where(n => n.Gioi == true);
                    break;
                case "today":
                    queryable = queryable.Where(n => n.NgayKhamDau.HasValue &&
                        n.NgayKhamDau.Value.Date == currentDay.Date);
                    break;
            }

            return Ok(await queryable.ToListAsync());
        }

        // GET: api/BenhNhan/{id}
        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BenhNhan>> GetBenhNhan(string id)
        {
            var benhNhan = await _context.BenhNhans.FindAsync(id);

            if (benhNhan == null)
            {
                return NotFound();
            }

            return Ok(benhNhan);
        }

        // POST: api/BenhNhan/Move/{id}
        // Only accessible by Admin and Staff
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost("Move/{id}")]
        public async Task<IActionResult> Move(string id)
        {
            var benhnhan = await _context.BenhNhans.FirstOrDefaultAsync(m => m.IdbenhNhan == id);

            if (benhnhan == null)
            {
                return NotFound();
            }

            Random random = new Random();
            int randomNumber = random.Next(100, 999);

            DanhSachKham ds = new DanhSachKham
            {
                IdbenhNhan = benhnhan.IdbenhNhan,
                NgayKham = DateTime.Now,
                Idkham = randomNumber.ToString()
            };

            await _context.DanhSachKhams.AddAsync(ds);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/BenhNhan
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<ActionResult<BenhNhan>> PostBenhNhan(BenhNhan benhNhan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BenhNhans.Add(benhNhan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBenhNhan), new { id = benhNhan.IdbenhNhan }, benhNhan);
        }

        // PUT: api/BenhNhan/{id}
        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBenhNhan(string id, BenhNhan benhNhan)
        {
            if (id != benhNhan.IdbenhNhan)
            {
                return BadRequest();
            }

            _context.Entry(benhNhan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BenhNhanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/BenhNhan/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBenhNhan(string id)
        {
            var benhNhan = await _context.BenhNhans.FindAsync(id);

            if (benhNhan == null)
            {
                return NotFound();
            }

            _context.BenhNhans.Remove(benhNhan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BenhNhanExists(string id)
        {
            return _context.BenhNhans.Any(e => e.IdbenhNhan == id);
        }
    }
}
