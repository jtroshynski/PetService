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
        //***NOTE: Change this file path to your local copy of the Pets.db file to make the program runs correctly.
        private string databasePath = @"D:\development\workspace\PetService\Pets.db";

        /// <summary>
        /// Gets all pets based on the given filter. Can only filter one parameter per call.
        /// If no filter is given, will return all pets in the database.
        /// http://localhost:59929/api/pets
        /// </summary>
        /// <param name="count">Boolean. If true, will return a count of the Pets in the db with the given filters. If false, will return a list of the Pet objects</param>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <param name="sex"></param>
        /// <param name="description"></param>
        /// <param name="ownerEmail"></param>
        /// <param name="ownerPrimaryPhone"></param>
        /// <param name="imageURL"></param>
        /// <returns></returns>
        public IHttpActionResult GetPets(bool count = false, string name = null, int? age = null, string sex = null, string description = null, string ownerEmail = null, string ownerPrimaryPhone = null, string imageURL = null)
        {
            var mapper = new BsonMapper();
            mapper.IncludeFields = true;
            // Open database (or create if it doesn't exist)
            using (var db = new LiteDatabase(databasePath, mapper))
            {
                var pets = db.GetCollection<Pet>("pets");
                BsonMapper.Global.IncludeFields = true;

                //Find the non-null filter item
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

                //Get pets using filter query
                if (filterQuery != null)
                {
                    if (count == true)
                    {
                        return Ok(pets.Count(filterQuery));
                    }

                    return Ok(pets.Find(filterQuery).ToList());
                }
                else //Get all pets, no filter query was given
                {
                    if (count == true)
                    {
                        return Ok(pets.Count());
                    }

                    return Ok(pets.FindAll());
                }

            }
        }

        /// <summary>
        /// Gets a single pet by id
        /// http://localhost:59929/api/pets?id=1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// POST: http://localhost:59929/api/pets
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
        /// PUT: http://localhost:59929/api/pets?id=1
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

        /// <summary>
        /// Deletes the pet at a given index
        /// http://localhost:59929/api/pets?id=1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
