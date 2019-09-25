using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using PetService.Models;

namespace PetService.Controllers
{
    public class PetController : ApiController
    {
        Pet[] pets = new Pet[]
        {
            new Pet { id = 1, name = "Baba", species = "Rabbit", age = 1, sex = "M"},
            new Pet { id = 2, name = "Petunia", species = "Cat", age = 1, sex = "F" },
            new Pet { id = 3, name = "Mongo", species = "Dog", age = 1, sex = "M" }
        };

        public IEnumerable<Pet> GetAllPets()
        {
            return pets;
        }

        public IHttpActionResult GetPet(int id)
        {
            var pet = pets.FirstOrDefault((p) => p.id == id);
            if (pet == null)
            {
                return NotFound();
            }
            return Ok(pet);
        }
    }
}
