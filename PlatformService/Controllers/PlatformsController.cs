using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo repo, IMapper mapper,
            ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        private static int requestCounter = 0;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetPlatforms()
        {
            requestCounter++;
            Console.WriteLine($"--> Request Counter {requestCounter}");
            if (requestCounter <= 2)
            {
                Console.WriteLine($"Request counter: {requestCounter} - delaying...");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }

            if (requestCounter <= 4)
            {
                Console.WriteLine($"Request counter: {requestCounter}: 500 (internal server error)");
                return StatusCode(500);
            }
            Console.WriteLine($"Request counter: {requestCounter}: 200 (ok)");

            Console.WriteLine("--> Getting Platforms....");
            var platformItem = _repo.GetAllPlatforms();
            var platformReadDtoList = _mapper.Map<IEnumerable<PlatformReadDto>>(platformItem);
            return Ok(platformReadDtoList);
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            /*var platformItem = _repo.GetPlatformById(id);
            if (platformItem != null)
            {
                return Ok(new PlatformReadDto
                {
                    Id = platformItem.Id,
                    Name = platformItem.Name,
                    Publisher = platformItem.Publisher,
                    Cost = platformItem.Cost
                });
            }
            return NotFound();*/
            var platformItem = _repo.GetPlatformById(id);
            if (platformItem != null)
            {
                var platformReadDto = _mapper.Map<PlatformReadDto>(platformItem);
                return Ok(platformReadDto);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            /* var platformModel = new Platform
             {
                 Name = platformCreateDto.Name,
                 Publisher = platformCreateDto.Publisher,
                 Cost = platformCreateDto.Cost
             };
             _repo.CreatePlatform(platformModel);
             _repo.SaveChanges();

             var platformReadDto = new PlatformReadDto
             {
                 Id = platformModel.Id,
                 Name = platformModel.Name,
                 Publisher = platformModel.Publisher,
                 Cost = platformModel.Cost
             };
             return CreatedAtRoute(nameof(GetPlatformById),
                 new { Id = platformReadDto.Id }, platformReadDto);*/
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repo.CreatePlatform(platformModel);
            _repo.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                // Send Sync Message
                //await _commandDataClient.SendPlatformToCommand(platformReadDto);

                //send async message    
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById),
                new { Id = platformReadDto.Id }, platformReadDto);
        }

    }
}