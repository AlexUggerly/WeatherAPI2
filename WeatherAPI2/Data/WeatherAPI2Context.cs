using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherAPI2.Models;

namespace WeatherAPI2.Data
{
    public class WeatherAPI2Context : DbContext
    {
        public WeatherAPI2Context (DbContextOptions<WeatherAPI2Context> options)
            : base(options)
        {
        }

        public DbSet<WeatherAPI2.Models.WeatherForecast> WeatherForecast { get; set; } = default!;
    }
}
