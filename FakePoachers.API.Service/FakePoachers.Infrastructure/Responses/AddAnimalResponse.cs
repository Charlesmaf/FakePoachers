using FakePoachers.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakePoachers.Infrastructure.Responses
{
    public class AddAnimalResponse : BaseResponse
    {
        public int Id { get; set; }
    }
}
