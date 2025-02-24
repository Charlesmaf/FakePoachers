using FakePoachers.Infrastructure.Contracts;
using FakePoachers.Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FakePoachers.API.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<LocationController> _logger;
        public LocationController(ILocationService locationService, ILogger<LocationController> logger)
        {
            this._locationService = locationService;
            _logger = logger;
        }

        [HttpGet("GetLocations")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<ActionResult<GetLocationsResponse>> GetLocations()
        {
            var response = await _locationService.GetLocations();

            return Ok(response);
        }
    }
}
