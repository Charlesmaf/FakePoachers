using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakePoachers.Infrastructure.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Address { get; set; }
        public int FkAnimalId { get; set; }

        [ForeignKey("FkAnimalId")]
        public Animal Animal { get; set; }
    }
}
