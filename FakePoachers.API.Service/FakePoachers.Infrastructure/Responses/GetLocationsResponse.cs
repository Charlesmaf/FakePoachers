using FakePoachers.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakePoachers.Infrastructure.Responses
{
    public class GetLocationsResponse : BaseResponse
    {
        public List<LocationDTO>? Content { get; set; }
    }
}
