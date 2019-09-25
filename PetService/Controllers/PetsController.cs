using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiteDB;
using PetService.Models;

namespace PetService.Controllers
{
    public class PetsController : ApiController
    {
        Pet[] pets = new Pet[]
        {
            new Pet { id = 1, name = "Baba", species = "Rabbit", age = 1, sex = "M"},
            new Pet { id = 2, name = "Petunia", species = "Cat", age = 1, sex = "F" },
            new Pet { id = 3, name = "Mongo", species = "Dog", age = 1, sex = "M" }
        };

        //public IEnumerable<Pet> GetAllPets()
        //{
        //    return pets;
        //}

        public IHttpActionResult GetAllPets()
        {
            // Open database (or create if not exits)
            using (var db = new LiteDatabase(@"D:\development\workspace\PetService\Pets.db"))
            {
                // Get customer collection
                var pets = db.GetCollection<Pet>("pets");

                // Create your new customer instance
                Pet pet = new Pet(){ name = "Baba", species = "Rabbit", age = 1, sex = "M" };

                pets.Insert(pet);

                // Update a document inside a collection
                pet.name = "Petunia2";

                pets.Update(pet);

                // Index document using a document property
                pets.EnsureIndex(x => x.name);

                // Use Linq to query documents
                var results = pets.Find(x => x.name.StartsWith("P"));

                return Ok(results);
            }
            //var pet = pets.FirstOrDefault((p) => p.id == id);
            //if (pet == null)
            //{
            //    return NotFound();
            //}
            
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
