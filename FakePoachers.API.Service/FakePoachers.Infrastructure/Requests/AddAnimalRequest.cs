using FakePoachers.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakePoachers.Infrastructure.Requests
{
    public class AddAnimalRequest
    {
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public IFormFile ImageFile { get; set; }

    }
}
