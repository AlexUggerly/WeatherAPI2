using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeatherAPI2.Data;
using WeatherAPI2.Models;

namespace WeatherAPI2.Controllers
{
    public class WeatherForecastsController : Controller
    {
        private readonly WeatherAPI2Context _context;

        public WeatherForecastsController(WeatherAPI2Context context)
        {
            _context = context;
        }
        public async Task<WeatherForecast> Nicklas(int lon, int lat)
        {

            using HttpClient todoClient = new();


                todoClient.BaseAddress = new Uri("https://localhost:7269/Forecasts");

            
            using HttpResponseMessage response = await todoClient.GetAsync($"?id=1");

            response.EnsureSuccessStatusCode();


            var jsonResponse = await response.Content.ReadAsStringAsync();

            WeatherForecast? wfc = JsonSerializer.Deserialize<WeatherForecast>(jsonResponse);
            return wfc;

        }
       

        // GET: WeatherForecasts
        public async Task<IActionResult> Index()
        {
            List<WeatherForecast> WeatherForecastList = new();
            WeatherForecastList.Add(Nicklas(12, 55).Result);
            WeatherForecastList.Add(Nicklas(23, 66).Result);
            WeatherForecastList.Add(Nicklas(72, 33).Result);
            WeatherForecastList.Add(Nicklas(180, 89).Result);
            return View(WeatherForecastList);
        }

        // GET: WeatherForecasts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WeatherForecast == null)
            {
                return NotFound();
            }

            var WeatherForecast = await _context.WeatherForecast
                .FirstOrDefaultAsync(m => m.Id == id);
            if (WeatherForecast == null)
            {
                return NotFound();
            }

            return View(WeatherForecast);
        }

        // GET: WeatherForecasts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WeatherForecasts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Main,Description,Icon")] WeatherForecast WeatherForecast)
        {
            if (ModelState.IsValid)
            {
                _context.Add(WeatherForecast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(WeatherForecast);
        }

        // GET: WeatherForecasts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WeatherForecast == null)
            {
                return NotFound();
            }

            var WeatherForecast = await _context.WeatherForecast.FindAsync(id);
            if (WeatherForecast == null)
            {
                return NotFound();
            }
            return View(WeatherForecast);
        }

        // POST: WeatherForecasts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Main,Description,Icon")] WeatherForecast WeatherForecast)
        {
            if (id != WeatherForecast.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(WeatherForecast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeatherForecastExists(WeatherForecast.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(WeatherForecast);
        }

        // GET: WeatherForecasts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WeatherForecast == null)
            {
                return NotFound();
            }

            var WeatherForecast = await _context.WeatherForecast
                .FirstOrDefaultAsync(m => m.Id == id);
            if (WeatherForecast == null)
            {
                return NotFound();
            }

            return View(WeatherForecast);
        }

        // POST: WeatherForecasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WeatherForecast == null)
            {
                return Problem("Entity set 'WeatherAPI2Context.WeatherForecast'  is null.");
            }
            var WeatherForecast = await _context.WeatherForecast.FindAsync(id);
            if (WeatherForecast != null)
            {
                _context.WeatherForecast.Remove(WeatherForecast);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeatherForecastExists(int id)
        {
          return _context.WeatherForecast.Any(e => e.Id == id);
        }
    }
}
