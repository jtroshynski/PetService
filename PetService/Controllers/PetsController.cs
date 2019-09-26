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
        private string databasePath = @"C:\development\Solutions\Pets.db";

        //api/pets
        public IHttpActionResult GetAllPets()
        {
            var mapper = new BsonMapper();
            mapper.IncludeFields = true;
            // Open database (or create if it doesn't exist)
            using (var db = new LiteDatabase(databasePath, mapper))
            {
                // Get pet collection
                var pets = db.GetCollection<Pet>("pets");
                BsonMapper.Global.IncludeFields = true;

                var results = pets.FindAll();

                if (results != null)
                {
                    return Ok(results.ToList());
                }
                else
                {
                    return NotFound();
                }
            }

        }
        //api/pets?id = 1
        public IHttpActionResult Get(int id)
        {
            var mapper = new BsonMapper();
            mapper.IncludeFields = true;
            // Open database (or create if it doesn't exist)
            using (var db = new LiteDatabase(databasePath, mapper))
            {
                // Get pet collection
                var pets = db.GetCollection<Pet>("pets");
                BsonMapper.Global.IncludeFields = true;

                var results = pets.Find(x => x.Id == id);

                if (results != null)
                {
                    return Ok(results.ToList());
                }
                else
                {
                    return NotFound();
                }
            }
        }

        // POST: api/pets
        public IHttpActionResult Post(List<Pet> pets)
        {
            var mapper = new BsonMapper();
            mapper.IncludeFields = true;
            // Open database (or create if it doesn't exist)
            using (var db = new LiteDatabase(databasePath, mapper))
            {
                // Get pet collection
                var petsDb = db.GetCollection<Pet>("pets");

                List<int> ids = new List<int>();

                foreach (Pet pet in pets)
                {
                    ids.Add(petsDb.Insert(pet));
                }
                petsDb.EnsureIndex(x => x.name);

                var results = pets.FindAll(x => ids.Contains(x.Id));

                return Ok(results.ToList());
                
            }
        }

        // PUT: api/pets?id=1
        public IHttpActionResult Put(int id, [FromBody]Pet pet)
        {
            var mapper = new BsonMapper();
            mapper.IncludeFields = true;
            // Open database (or create if it doesn't exist)
            using (var db = new LiteDatabase(databasePath, mapper))
            {
                // Get pet collection
                var pets = db.GetCollection<Pet>("pets");

                if (pets.Exists(x => x.Id == id))
                {
                    return BadRequest("Item already exists at with the given id: " + id);
                }

                pet.Id = id;
                pets.Insert(pet);

                return Ok("Successful Insertion");

            }
        }

        // DELETE: api/pets?id=1
        public IHttpActionResult Delete([FromBody]int id)
        {
            var mapper = new BsonMapper();
            mapper.IncludeFields = true;
            // Open database (or create if it doesn't exist)
            using (var db = new LiteDatabase(databasePath, mapper))
            {
                // Get pet collection
                var pets = db.GetCollection<Pet>("pets");

                // Use Linq to query documents
                var results = pets.Delete(id);

                return Ok("Successful deletion");

            }
        }
    }
}
