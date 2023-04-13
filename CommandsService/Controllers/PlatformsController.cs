using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using CommandsService.SyncDataServices;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepo platformRepo,
            ICommandRepo commandRepo, IMapper mapper)
        {
            _platformRepo = platformRepo;
            _commandRepo = commandRepo;
            _mapper = mapper;
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
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from CommandsService");
            var platformItems = _commandRepo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpPost]
        public async Task<ActionResult> TestInboundConnection(PlatformReadDto platformReadDto)
        {
            Console.WriteLine("--> Tambahkan platform baru");
            try
            {
                await _platformRepo.CreatePlatform(new Platform
                {
                    Id = platformReadDto.Id,
                    Name = platformReadDto.Name
                });
                return Ok(platformReadDto);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Could not add platform to the database: {ex.Message}");
            }
        }
    }

}