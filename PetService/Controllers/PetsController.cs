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

        //api/pets
        public IHttpActionResult GetPets(string name = null, int? age = null, string sex = null, string description = null, string ownerEmail = null, string ownerPrimaryPhone = null, string imageURL = null)
        {
            var mapper = new BsonMapper();
            mapper.IncludeFields = true;
            // Open database (or create if it doesn't exist)
            using (var db = new LiteDatabase(databasePath, mapper))
            {
                var pets = db.GetCollection<Pet>("pets");
                BsonMapper.Global.IncludeFields = true;

                Query filterQuery = null;

                if (name != null)
                {
                    filterQuery = Query.EQ("name", name);
                }
                else if (age != null)
                {
                    filterQuery = Query.EQ("age", age);
                }
                else if (sex != null)
                {
                    filterQuery = Query.EQ("sex", sex);
                }
                else if (description != null)
                {
                    filterQuery = Query.EQ("description", description);
                }
                else if (ownerEmail != null)
                {
                    filterQuery = Query.EQ("ownerEmail", ownerEmail);
                }
                else if (ownerPrimaryPhone != null)
                {
                    filterQuery = Query.EQ("ownerPrimaryPhone", ownerPrimaryPhone);
                }
                else if (imageURL != null)
                {
                    filterQuery = Query.EQ("imageURL", imageURL);
                }

                IEnumerable<Pet> results;

                if (filterQuery != null)
                {
                    results = pets.Find(filterQuery);
                }
                else
                {
                    results = pets.FindAll();
                }

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

        /// <summary>
        /// Inserts pets in the database with the next available id 
        /// POST: api/pets
        /// </summary>
        /// <param name="pets"></param>
        /// <returns></returns>
        public IHttpActionResult Post(List<Pet> pets)
        {
            var mapper = new BsonMapper();
            mapper.IncludeFields = true;
            // Open database (or create if it doesn't exist)
            using (var db = new LiteDatabase(databasePath, mapper))
            {
                var petsDb = db.GetCollection<Pet>("pets");

                List<int?> ids = new List<int?>();

                foreach (Pet pet in pets)
                {
                    pet.Id = null; //make sure id is removed, so we don't overwrite an existing pet
                    ids.Add(petsDb.Insert(pet)); //add ids returned from insertion, to return updated pets
                }
                petsDb.EnsureIndex(x => x.name);

                var results = pets.FindAll(x => ids.Contains(x.Id));

                return Ok(results.ToList());
            }
        }

        /// <summary>
        /// Inserts or updates pets with Ids
        /// PUT: api/pets?id=1
        /// </summary>
        /// <param name="pets">List of pet objects, must have Ids.</param>
        /// <returns></returns>
        public IHttpActionResult Put(List<Pet> pets)
        {
            var mapper = new BsonMapper();
            mapper.IncludeFields = true;
            // Open database (or create if it doesn't exist)
            using (var db = new LiteDatabase(databasePath, mapper))
            {
                var petsDb = db.GetCollection<Pet>("pets");

                foreach (Pet pet in pets )
                {
                    if (pet.Id == null)
                    {
                        return BadRequest("All pets must have Ids. Some pets may have been updated.");
                    }
                    if (petsDb.Exists(x => x.Id == pet.Id))
                    {
                        petsDb.Update(pet);
                    }
                    else
                    {
                        petsDb.Insert(pet);
                    }
                }

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
                var pets = db.GetCollection<Pet>("pets");

                pets.Delete(id);

                return Ok("Successful deletion");
            }
        }
    }
}
