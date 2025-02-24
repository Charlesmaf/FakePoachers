using System.ComponentModel.DataAnnotations;

namespace FakePoachers.Infrastructure.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public byte[]? ImageData { get; set; }
        public Location Location { get; set; }
    }
}
