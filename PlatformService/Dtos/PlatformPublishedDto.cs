using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Dtos
{
    public class PlatformPublishedDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Event { get; set; } = string.Empty;
    }
}