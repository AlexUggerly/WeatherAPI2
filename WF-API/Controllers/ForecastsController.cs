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
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Forecast>>> GetForecast()
        //{
        //    return await _context.Forecast.ToListAsync();
        //}

        // GET: api/Forecasts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SimpleForecast>> GetForecast(int id)
        {
            Root forecast = APIGetForecast(10, 55).Result;
            SimpleForecast sf = new SimpleForecast() {Id=forecast.id, Description=forecast.weather[0].description, Icon = forecast.weather[0].icon};

            if (forecast == null)
            {
                return NotFound();
            }

            return sf;
        }

        private async Task<Root> APIGetForecast(int lon, int lat)
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
