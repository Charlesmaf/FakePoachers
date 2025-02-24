using FakePoachers.Infrastructure.Contracts;
using FakePoachers.Infrastructure.DTOs;
using FakePoachers.Infrastructure.Models;
using FakePoachers.Infrastructure.Requests;
using FakePoachers.Infrastructure.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Globalization;
using System.Net;

namespace FakePoachers.Library.Service
{
    /// <summary>
    /// Service for managing animal-related operations.
    /// </summary>
    public class AnimalService : IAnimalService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        private const string AnimalsCacheKey = "animals_cache";
        private const string AnimalCacheKeyPrefix = "animal_";

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimalService"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="logger">The logger instance.</param>
        /// <param name="cache">The memory cache instance.</param>
        public AnimalService(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Adds a new animal to the database.
        /// </summary>
        /// <param name="request">The request containing animal details.</param>
        /// <returns>A response indicating the result of the add operation.</returns>
        public async Task<AddAnimalResponse> AddAnimal(AddAnimalRequest request)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await request.ImageFile.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                if (!double.TryParse(request.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude) ||
                    !double.TryParse(request.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude))
                {
                    return new AddAnimalResponse
                    {
                        ResponseCode = (int)HttpStatusCode.BadRequest,
                        ResponseMessage = "Failed",
                        AdditionalInformation = "Invalid latitude or longitude format."
                    };
                }

                var animal = new Animal
                {
                    Name = request.Name,
                    ImageData = imageData,
                    Location = new Location
                    {
                        Latitude = latitude,
                        Longitude = longitude,
                        Address = request.Address
                    }
                };

                _context.Animals.Add(animal);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _cache.Remove(AnimalsCacheKey);
                    return new AddAnimalResponse
                    {
                        Id = animal.Id,
                        Result = result,
                        ResponseCode = (int)HttpStatusCode.OK,
                        ResponseMessage = "Success",
                        AdditionalInformation = "Request was successful",
                    };
                }

                return new AddAnimalResponse
                {
                    ResponseCode = (int)HttpStatusCode.BadRequest,
                    ResponseMessage = "Failed",
                    AdditionalInformation = "Failed to process request",
                };
            }
            catch (DbUpdateException dbEx)
            {
                Log.Error(dbEx, "Database error while saving animal.");
                return new AddAnimalResponse
                {
                    ResponseCode = (int)HttpStatusCode.InternalServerError,
                    ResponseMessage = "Failed",
                    AdditionalInformation = "Database error occurred. Please try again later."
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error occurred while adding an animal.");
                return new AddAnimalResponse
                {
                    ResponseCode = (int)HttpStatusCode.InternalServerError,
                    ResponseMessage = "Failed",
                    AdditionalInformation = "An unexpected error occurred. Please contact support."
                };
            }
        }

        /// <summary>
        /// Retrieves a paginated list of animals.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of animals per page.</param>
        /// <returns>A response containing the list of animals.</returns>
        public async Task<GetAnimalsResponse> GetAnimals(int page = 1, int pageSize = 10)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                {
                    throw new ArgumentException("Page and pageSize must be greater than zero.");
                }
                string cacheKey = $"{AnimalsCacheKey}_{page}_{pageSize}";
                if (_cache.TryGetValue(cacheKey, out GetAnimalsResponse cachedAnimals))
                {
                    Log.Information("Returning cached animal list for page {Page}", page);
                    return cachedAnimals;
                }
                int totalRecords = await _context.Animals.CountAsync();
                var animals = await _context.Animals
                    .AsNoTracking()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(a => new AnimalListDTO
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Image = a.ImageData
                    })
                    .ToListAsync();

                var response = new GetAnimalsResponse
                {
                    Content = animals,
                    Result = totalRecords,
                    ResponseCode = animals.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NoContent,
                    ResponseMessage = animals.Any() ? "Success" : "No animals found",
                    AdditionalInformation = "Request was successful"
                };

                _cache.Set(cacheKey, response, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
                Log.Information("Cached animal list for page {Page}", page);

                return response;
            }
            catch (ArgumentException ex)
            {
                Log.Error(ex, "Invalid pagination parameters: page={Page}, pageSize={PageSize}", page, pageSize);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving animals.");
                throw new ApplicationException("An error occurred while retrieving animals. Please try again later.", ex);
            }
        }

        /// <summary>
        /// Retrieves the details of a specific animal by its ID.
        /// </summary>
        /// <param name="id">The ID of the animal to retrieve.</param>
        /// <returns>A response containing the animal details.</returns>
        public async Task<GetAnimalDetailsResponse> GetAnimalById(int id)
        {
            try
            {

                string cacheKey = $"{AnimalCacheKeyPrefix}{id}";
                if (_cache.TryGetValue(cacheKey, out GetAnimalDetailsResponse cachedAnimal))
                {
                    Log.Information("Returning cached details for animal ID {Id}", id);
                    return cachedAnimal;
                }
                var animal = await _context.Animals
                    .AsNoTracking()
                    .Include(a => a.Location)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (animal == null)
                {
                    Log.Warning($"Animal with ID {id} not found.");
                    return new GetAnimalDetailsResponse
                    {
                        ResponseCode = (int)HttpStatusCode.NotFound,
                        ResponseMessage = "NotFound",
                        AdditionalInformation = $"Animal with ID {id} not found"
                    };
                }
                var response = new GetAnimalDetailsResponse
                {
                    Content = new AnimalDetailsDTO
                    {
                        Id = animal.Id,
                        Name = animal.Name,
                        Image = animal.ImageData != null ? Convert.ToBase64String(animal.ImageData) : null,
                        Latitude = animal.Location?.Latitude ?? 0,
                        Longitude = animal.Location?.Longitude ?? 0,
                    },
                    ResponseCode = (int)HttpStatusCode.OK,
                    ResponseMessage = "Success",
                    AdditionalInformation = "Request was successful"
                };

                _cache.Set(cacheKey, response, TimeSpan.FromMinutes(15)); // Cache for 15 minutes
                Log.Information("Cached details for animal ID {Id}", id);

                return response;
            }
            catch (ArgumentException ex)
            {
                Log.Warning(ex, $"Invalid animal ID provided: {id}");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while retrieving the animal with ID {id}.");
                throw new ApplicationException("An error occurred while retrieving animal details. Please try again later.", ex);
            }
        }
    }
}
