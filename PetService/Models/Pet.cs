using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetService.Models
{
    public class Pet
    {
        public Pet() { }
        //species of pet, it’s name, age, sex, description, pet owner’s contact info, and an image URL
        [BsonId]
        public int? Id;

        public string name;
        public string species;
        public int age;
        public string sex;
        public string description;
        public string ownerEmail;
        public string ownerPrimaryPhone;
        public string imageURL;
        
    }
}