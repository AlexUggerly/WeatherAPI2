using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WF_API.Data;
using WF_API.Models;

namespace WF_API.Controllers
{
    [Route("api/Forecasts")]
    [ApiController]
    public class ForecastsController : ControllerBase
    {
        private readonly WF_APIContext _context;

        public ForecastsController(WF_APIContext context)
        {
            _context = context;
        }

        // GET: api/Forecasts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Forecast>>> GetForecast()
        {
            return await _context.Forecast.ToListAsync();
        }

        // GET: api/Forecasts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SimpleForecast>> GetForecast(int id)
        {
            Root forecast = GetForeCasts(10, 55).Result;
            SimpleForecast sf = new SimpleForecast() {Id=forecast.id, Description=forecast.weather[0].description, Icon = forecast.weather[0].icon};

            if (forecast == null)
            {
                return NotFound();
            }

            return sf;
        }

        // PUT: api/Forecasts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForecast(int id, Forecast forecast)
        {
            if (id != forecast.Id)
            {
                return BadRequest();
            }

            _context.Entry(forecast).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForecastExists(id))
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

        // POST: api/Forecasts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Forecast>> PostForecast(Forecast forecast)
        {
            _context.Forecast.Add(forecast);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetForecast), new { id = forecast.Id }, forecast);
        }

        // DELETE: api/Forecasts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForecast(int id)
        {
            var forecast = await _context.Forecast.FindAsync(id);
            if (forecast == null)
            {
                return NotFound();
            }

            _context.Forecast.Remove(forecast);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ForecastExists(int id)
        {
            return _context.Forecast.Any(e => e.Id == id);
        }
        public async Task<Root> GetForeCasts(int lon, int lat)
        {

            using HttpClient todoClient = new();


            todoClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/weather");


            using HttpResponseMessage response = await todoClient.GetAsync($"?lat={lat}&lon={lon}&appid=8a4b4911a23641ac49470b87984939f0");

            response.EnsureSuccessStatusCode();


            var jsonResponse = await response.Content.ReadAsStringAsync();

            Root? wfc = JsonSerializer.Deserialize<Root>(jsonResponse);
            return wfc;

        }
    }
}
