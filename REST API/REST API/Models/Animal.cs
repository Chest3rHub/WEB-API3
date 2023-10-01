using System.ComponentModel.DataAnnotations;

namespace REST_API.Models;

public class Animal
{
    [Required]
    public int IdAnimal { get; set; }
    [Required]
    public string Name { get; set; }
        
#nullable enable
    public string ? Description { get; set; }
    [Required]
    public string Category { get; set; }
    [Required]
    public string Area { get; set; }
    // wartosci ktore nie zawsze musza miec wartosc zaznaczamy
    // jako nullable czyli znak zapytania po string
    // string ? nazwa
    // mozna tez dwa razy kliknac na projekt i enable na disable zmieniamy
    // w opcji nullable
}