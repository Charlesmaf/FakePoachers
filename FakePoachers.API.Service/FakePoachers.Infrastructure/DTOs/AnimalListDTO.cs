using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakePoachers.Infrastructure.DTOs
{
    public class AnimalListDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public byte[]? Image { get; set; }
    }
}
