# PetService
An example API for getting, updating, and deleting pets built using ASP.NET and LiteDb.

To test, please follow the following steps:

1. Clone the repository using this url: https://github.com/jtroshynski/PetService.git

2. Change the filepath of the database file using the following steps

    -Open the file Controllers/PetController.cs

    -Change the variable databaseFilePath to point to your local copy of the Pets.db file database, given as part of the project.
    
    Ex. databasePath = @"D:/development/workspace/PetService/Pets.db"

3. Run the "PetStore.html" file to start your local web service.

4. Use the following Postman Collection for testing the various functions

    -https://www.getpostman.com/collections/5cc07c6dbcf353534c3f
  
    NOTE: For the GetPetsFiltered postman function, I only wrote it to handle one filter type at a time, so you'll have to access the Params and select which filter type you'd like to use.

