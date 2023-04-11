using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommandsService.Dtos;

namespace CommandsService.SyncDataServices
{
    public class HttpPlatformDataClient : IPlatformDataClient
    {
        private readonly HttpClient _httpClient;
        public HttpPlatformDataClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<PlatformReadDto>> ReturnAllPlatforms()
        {
            var response = await _httpClient.GetAsync("http://localhost:6000/api/platforms");
            if (response.IsSuccessStatusCode)
            {

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{content}");
                var platforms = JsonSerializer.Deserialize<List<PlatformReadDto>>(content);
                if (platforms != null)
                {
                    Console.WriteLine($"{platforms.Count()} platforms returned from Platforms Service");
                    return platforms;
                }
                throw new Exception("No platforms found");
            }
            else
            {
                throw new Exception("Unable to reach Platforms Service");
            }
        }
    }
}