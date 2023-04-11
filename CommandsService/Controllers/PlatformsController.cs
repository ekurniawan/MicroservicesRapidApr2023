using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.SyncDataServices;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;

        public PlatformsController(IPlatformRepo platformRepo)
        {
            _platformRepo = platformRepo;
        }

        [HttpPost("Sync")]
        public async Task<ActionResult> SyncPlatforms()
        {
            try
            {
                await _platformRepo.CreatePlatform();
                return Ok("Platforms Synced");
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not sync platforms: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platforms = await _platformRepo.GetAllPlatforms();
            return platforms.Select(p => new PlatformReadDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST Command Service");
            return Ok("Inbound test from Platforms Controller");
        }
    }

}