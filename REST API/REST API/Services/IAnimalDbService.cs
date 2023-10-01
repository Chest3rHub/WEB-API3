using REST_API.Models;

namespace REST_API.Services;

public interface IAnimalDbService
{
    //query parameter to co wystepuje po znaku zapytania w url
    Task <IList<Animal>> GetAnimalsListAsync(string orderBy);
    Task<int> AddAnimal(Animal animal);

    Task<int> UpdateAnimal(Animal animal, int idAnimal);

    Task<int> DeleteAnimal(int idAnimal);
}