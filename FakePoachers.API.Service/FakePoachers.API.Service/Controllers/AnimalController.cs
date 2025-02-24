using FakePoachers.Infrastructure.Contracts;
using FakePoachers.Infrastructure.Requests;
using FakePoachers.Infrastructure.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Serilog;

namespace FakePoachers.API.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalController : ControllerBase
    {

        private readonly IAnimalService _animalService;
        public AnimalController(IAnimalService animalService)
        {
            this._animalService = animalService;
        }
        [HttpPost("AddAnimal")]
        public async Task<ActionResult<AddAnimalResponse>> AddAnimal([FromForm] AddAnimalRequest request)
        {
            try
            {
                if (request == null || !ModelState.IsValid)
                {
                    Log.Error($"AddAnimal() : BadRequest(), request was null or empty");
                    return BadRequest();
                }
                if (request.ImageFile == null || request.ImageFile.Length == 0)
                {
                    return BadRequest("Image file is required.");
                }

                var result = await _animalService.AddAnimal(request);
                if (result.ResponseCode != (int)HttpStatusCode.OK)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Internal server error: {ex}");
                return this.Problem($"Internal server error: {ex}");
            }
        }

        [HttpGet("GetAnimals")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<ActionResult<GetAnimalsResponse>> GetAnimals(int page = 1, int pageSize = 10)
        {
            try
            {
                var result = await _animalService.GetAnimals(page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while retrieving animals.");
                return StatusCode(500, new { message = "An internal server error occurred. Please try again later." });
            }
        }

        [HttpGet("GetAnimalById/{id}")]
        public async Task<ActionResult<GetAnimalDetailsResponse>> GetAnimalById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    Log.Warning($"Invalid animal ID provided: {id}", id);
                    return BadRequest();
                }
                var result = await _animalService.GetAnimalById(id);
                if (result == null)
                {
                    Log.Warning($"Animal with ID {id} not found.", id);
                    return NotFound(new { message = "Animal not found." });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An unexpected error occurred while retrieving the animal with ID {id}.");
                return StatusCode(500, new { message = "An internal server error occurred. Please try again later." });
            }
        }
    }
}
