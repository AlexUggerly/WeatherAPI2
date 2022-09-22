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
    public class WeatherForeCastsController : Controller
    {
        private readonly WeatherAPI2Context _context;

        public WeatherForeCastsController(WeatherAPI2Context context)
        {
            _context = context;
        }
        public async Task<Root> Nicklas(int lon, int lat)
        {

            using HttpClient todoClient = new();


                todoClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/weather");

            
            using HttpResponseMessage response = await todoClient.GetAsync($"?lat={lat}&lon={lon}&appid=8a4b4911a23641ac49470b87984939f0");

            response.EnsureSuccessStatusCode();


            var jsonResponse = await response.Content.ReadAsStringAsync();

            Root? wfc = JsonSerializer.Deserialize<Root>(jsonResponse);
            return wfc;

        }
       

        // GET: WeatherForeCasts
        public async Task<IActionResult> Index()
        {
            List<Root> RootList = new();
            RootList.Add(Nicklas(12, 55).Result);
            RootList.Add(Nicklas(23, 66).Result);
            RootList.Add(Nicklas(72, 33).Result);
            RootList.Add(Nicklas(180, 89).Result);
            return View(RootList);
        }

        // GET: WeatherForeCasts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WeatherForeCast == null)
            {
                return NotFound();
            }

            var weatherForeCast = await _context.WeatherForeCast
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weatherForeCast == null)
            {
                return NotFound();
            }

            return View(weatherForeCast);
        }

        // GET: WeatherForeCasts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WeatherForeCasts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Main,Description,Icon")] WeatherForeCast weatherForeCast)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weatherForeCast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(weatherForeCast);
        }

        // GET: WeatherForeCasts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WeatherForeCast == null)
            {
                return NotFound();
            }

            var weatherForeCast = await _context.WeatherForeCast.FindAsync(id);
            if (weatherForeCast == null)
            {
                return NotFound();
            }
            return View(weatherForeCast);
        }

        // POST: WeatherForeCasts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Main,Description,Icon")] WeatherForeCast weatherForeCast)
        {
            if (id != weatherForeCast.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weatherForeCast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeatherForeCastExists(weatherForeCast.Id))
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
            return View(weatherForeCast);
        }

        // GET: WeatherForeCasts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WeatherForeCast == null)
            {
                return NotFound();
            }

            var weatherForeCast = await _context.WeatherForeCast
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weatherForeCast == null)
            {
                return NotFound();
            }

            return View(weatherForeCast);
        }

        // POST: WeatherForeCasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WeatherForeCast == null)
            {
                return Problem("Entity set 'WeatherAPI2Context.WeatherForeCast'  is null.");
            }
            var weatherForeCast = await _context.WeatherForeCast.FindAsync(id);
            if (weatherForeCast != null)
            {
                _context.WeatherForeCast.Remove(weatherForeCast);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeatherForeCastExists(int id)
        {
          return _context.WeatherForeCast.Any(e => e.Id == id);
        }
    }
}
