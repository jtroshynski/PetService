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
        private string databasePath = @"D:\development\workspace\PetService\Pets.db";
        //Pet[] pets = new Pet[]
        //{
        //    new Pet {name = "Baba", species = "Rabbit", age = 1, sex = "M"},
        //    new Pet {name = "Petunia", species = "Cat", age = 1, sex = "F" },
        //    new Pet {name = "Mongo", species = "Dog", age = 1, sex = "M" }
        //};


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

                Pet pet = new Pet(){ name = "Mongo", species = "Dog", age = 2, sex = "M" };

                pets.Insert(pet);

                pets.EnsureIndex(x => x.name);

                // Use Linq to query documents
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

                var results = pets.Find(x => x.name.StartsWith("D"));

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

        // POST: api/student
        public IHttpActionResult Post([FromBody]Pet pet)
        {
            var mapper = new BsonMapper();
            mapper.IncludeFields = true;
            // Open database (or create if it doesn't exist)
            using (var db = new LiteDatabase(databasePath, mapper))
            {
                // Get pet collection
                var pets = db.GetCollection<Pet>("pets");

                pets.Insert(pet);

                pets.EnsureIndex(x => x.name);

                // Use Linq to query documents
                var results = pets.Find(x => x.name.StartsWith("D"));

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

        // PUT: api/student/5
        public IHttpActionResult Put(int id, [FromBody]Pet pet)
        {
            var mapper = new BsonMapper();
            mapper.IncludeFields = true;
            // Open database (or create if it doesn't exist)
            using (var db = new LiteDatabase(databasePath, mapper))
            {
                // Get pet collection
                var pets = db.GetCollection<Pet>("pets");

                pet.Id = id;

                pets.Insert(pet);

                return Ok();

                //if (results != null)
                //{
                //    return Ok(results.ToList());
                //}
                //else
                //{
                //    return NotFound();
                //}
            }
        }

        // DELETE: api/student/5
        public IHttpActionResult Delete(int id)
        {
            var mapper = new BsonMapper();
            mapper.IncludeFields = true;
            // Open database (or create if it doesn't exist)
            using (var db = new LiteDatabase(databasePath, mapper))
            {
                // Get pet collection
                var pets = db.GetCollection<Pet>("pets");

                // Use Linq to query documents
                var results = pets.Find(x => x.Id == id);

                if (results != null)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                    //System.Web.Http.Results.
                }
            }
        }
    }
}
