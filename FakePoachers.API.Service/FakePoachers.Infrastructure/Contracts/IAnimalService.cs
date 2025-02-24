using FakePoachers.Infrastructure.Requests;
using FakePoachers.Infrastructure.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakePoachers.Infrastructure.Contracts
{
    public interface IAnimalService
    {
        Task<AddAnimalResponse> AddAnimal(AddAnimalRequest request);
        Task<GetAnimalsResponse> GetAnimals(int page = 1, int pageSize = 10);
        Task<GetAnimalDetailsResponse> GetAnimalById(int id);
    }
}
