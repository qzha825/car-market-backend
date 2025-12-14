using Microsoft.AspNetCore.Mvc;
using CarMarketBackend.Models;
using CarMarketBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CarMarketBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search)
        {
            var query = _context.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();

                query = query.Where(c =>
                    (c.Brand + " " + c.Model).ToLower().Contains(lowerSearch)
                );
            }

            var cars = await query.ToListAsync();
            return Ok(cars);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCar([FromBody] Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return Ok(car);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Car updatedCar)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            car.Brand = updatedCar.Brand;
            car.Model = updatedCar.Model;
            car.Year = updatedCar.Year;
            car.Mileage = updatedCar.Mileage;
            car.Price = updatedCar.Price;
            car.Image = updatedCar.Image;

            await _context.SaveChangesAsync();
            return Ok(car);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
