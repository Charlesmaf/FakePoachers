using FakePoachers.Infrastructure.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakePoachers.Infrastructure.Contracts
{
    public interface ILocationService
    {
        Task<GetLocationsResponse> GetLocations();
    }
}
