using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandsService.Dtos;

namespace CommandsService.SyncDataServices
{
    public interface IPlatformDataClient
    {
        Task<IEnumerable<PlatformReadDto>> ReturnAllPlatforms();
    }
}