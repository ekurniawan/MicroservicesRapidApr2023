using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandsService.Models;
using CommandsService.SyncDataServices;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;
        private readonly IPlatformDataClient _client;
        public PlatformRepo(AppDbContext context, IPlatformDataClient client)
        {
            _context = context;
            _client = client;
        }

        public async Task CreatePlatform()
        {
            var platforms = await _client.ReturnAllPlatforms();
            foreach (var plat in platforms)
            {
                _context.Add(new Platform
                {
                    Id = plat.Id,
                    Name = plat.Name
                });
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not save changes to the database: {ex.Message}");
            }
        }

        public async Task CreatePlatform(Platform plat)
        {
            try
            {
                if (plat == null)
                {
                    throw new ArgumentNullException(nameof(plat));
                }
                await _context.Platforms.AddAsync(plat);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Could not add platform to the database: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms()
        {
            return await _context.Platforms.ToListAsync();
        }
    }
}