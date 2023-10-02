using Microsoft.AspNetCore.Mvc;
using REST_API.Models;
using REST_API.Services;

namespace REST_API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    
    private IAnimalDbService _animalsDbService;

    public AnimalsController(IAnimalDbService animalsDbService)
    {
        _animalsDbService= animalsDbService;
    }
    [HttpGet]
    public async Task  <IActionResult> GetAnimalsOrderBy([FromQuery] string orderBy)
    {
        IList<Animal> animals= await _animalsDbService.GetAnimalsListAsync(orderBy);
        // metoda zwraca status jakis
        return Ok(animals);
    }
    [HttpPost]
    public async Task<IActionResult> AddAnimal([FromQuery] Animal animal)
    {
        int result = await _animalsDbService.AddAnimal(animal);
        if (result == 0)
        {
            return BadRequest();
        }
        return Ok("Added an animal");
    }
    [HttpPut("{idAnimal}")]
    public async Task<IActionResult> UpdateAnimal(Animal animal, int idAnimal)
    {
        int result = await _animalsDbService.UpdateAnimal(animal, idAnimal);
        if (result == 0)
        {
            return NotFound();
        }
        return Ok("Updated an existing animal");
    }
    [HttpDelete("{idAnimal}")]
    public async Task<IActionResult> DeleteAnimal(int idAnimal)
    {
        int result = await _animalsDbService.DeleteAnimal(idAnimal);
        if(result == 0)
        {
            return NotFound();
        }
        return Ok("Deleted an existing animal");
    }
}