using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        public PlatformsController(IPlatformRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var listPlatformReadDto = new List<PlatformReadDto>();
            Console.WriteLine("--> Getting Platforms....");
            var platformItem = _repo.GetAllPlatforms();
            foreach (var item in platformItem)
            {
                listPlatformReadDto.Add(new PlatformReadDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Publisher = item.Publisher,
                    Cost = item.Cost
                });
            }
            return Ok(listPlatformReadDto);
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _repo.GetPlatformById(id);
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
            return NotFound();
        }

    }
}