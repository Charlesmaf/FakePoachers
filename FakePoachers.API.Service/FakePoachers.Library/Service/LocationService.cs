using FakePoachers.Infrastructure.Contracts;
using FakePoachers.Infrastructure.DTOs;
using FakePoachers.Infrastructure.Models;
using FakePoachers.Infrastructure.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Net;

namespace FakePoachers.Library.Service
{
    /// <summary>
    /// Service for managing location-related operations.
    /// </summary>
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const string LocationsCacheKey = "locations_cache";

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationService"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="cache">The memory cache instance.</param>
        public LocationService(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Retrieves a list of locations.
        /// </summary>
        /// <returns>A response containing the list of locations.</returns>
        public async Task<GetLocationsResponse> GetLocations()
        {
            try
            {
                if (_cache.TryGetValue(LocationsCacheKey, out GetLocationsResponse cachedResponse))
                {
                    Log.Information("Returning cached location data.");
                    return cachedResponse;
                }

                Log.Information("Fetching location data from the database.");

                var locations = await _context.Locations
                    .AsNoTracking()
                    .Select(a => new LocationDTO
                    {
                        Latitude = a.Latitude,
                        Longitude = a.Longitude
                    })
                    .ToListAsync();

                if (!locations.Any())
                {
                    Log.Information("No locations found in the database.");
                    return new GetLocationsResponse()
                    {
                        ResponseCode = (int)HttpStatusCode.NoContent,
                        ResponseMessage = "NotFound",
                        AdditionalInformation = "No locations found"
                    };
                }

                var response = new GetLocationsResponse
                {
                    Content = locations,
                    ResponseCode = (int)HttpStatusCode.OK,
                    ResponseMessage = "Success",
                    AdditionalInformation = "Locations fetched successfully"
                };

                _cache.Set(LocationsCacheKey, response, TimeSpan.FromMinutes(15)); // Cache for 15 minutes
                Log.Information("Cached location data successfully.");

                return response;
            }
            catch (DbUpdateException dbEx)
            {
                Log.Error(dbEx, "Database error occurred while retrieving locations.");
                return new GetLocationsResponse
                {
                    ResponseCode = (int)HttpStatusCode.InternalServerError,
                    ResponseMessage = "Failed",
                    AdditionalInformation = "Database error occurred. Please try again later."
                };
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected error occurred while retrieving locations.");
                return new GetLocationsResponse
                {
                    ResponseCode = (int)HttpStatusCode.InternalServerError,
                    ResponseMessage = "Failed",
                    AdditionalInformation = "An unexpected error occurred. Please contact support."
                };
            }
        }
    }
}
