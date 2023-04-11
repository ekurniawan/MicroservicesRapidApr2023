using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandsService.Dtos;
using CommandsService.SyncDataServices;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformDataClient _platformDataClient;
        public PlatformsController(IPlatformDataClient platformDataClient)
        {
            _platformDataClient = platformDataClient;
        }

        [HttpGet]
        public async Task<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from Command Service");
            var platforms = await _platformDataClient.ReturnAllPlatforms();
            return platforms;
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST Command Service");
            return Ok("Inbound test from Platforms Controller");
        }
    }

}