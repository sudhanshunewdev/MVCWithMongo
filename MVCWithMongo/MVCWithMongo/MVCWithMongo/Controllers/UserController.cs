﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCWithMongo.Models;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MVCWithMongo.Controllers
{
    public class UserController : Controller
    {
        
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(UserModel um)
        {
            MongoClient client = new MongoClient("Server=localhost:27017");
            MongoServer server = client.GetServer();
            MongoDatabase database = server.GetDatabase("MVCTestDB1");

            //Connect to MongoDB
            //MongoServer objServer = MongoServer.Create("Server=localhost:27017");
            //Provide a database name(Mongo server will automatically check a 
            //database with this name while inserting.
            //if exist then ok otherwise it will create automatically create a database)
          //  MongoDatabase objDatabse = client.GetDatabase("MVCTestDB1");

           
            //Provide a table Name(Mongo will create a table with this name.
            //if its already exist then mongo will insert in this table otherwilse
            //it will create a table with this name and then Mongo will insert)
            MongoCollection<BsonDocument> UserDetails = database.GetCollection<BsonDocument>("MVCTestDB1");
            //Insert into Users table.
            BsonDocument objDocument = new BsonDocument {
                {"ID",um.ID},
                {"UserName",um.UserName},
                {"Password",um.Password},
                {"Email",um.Email},
                {"Address",um.Address}
                };

            UserDetails.Insert(objDocument);
            return RedirectToAction("GetUsers");
        }

        public ActionResult GetUsers()
        {
            MongoServer objServer = MongoServer.Create("Server=localhost:27017");
            MongoDatabase objDatabse = objServer.GetDatabase("MVCTestDB1");
            List<UserModel> UserDetails = objDatabse.GetCollection<UserModel>("MVCTestDB1").FindAll().ToList();
            return View(UserDetails);
        }

        public ActionResult Delete(int id)
        {
            MongoServer objServer = MongoServer.Create("Server=localhost:27017");
            MongoDatabase objDatabse = objServer.GetDatabase("MVCTestDB1");
            IMongoQuery query = Query.EQ("ID",id);
            objDatabse.GetCollection<UserModel>("MVCTestDB1").Remove(query);
            return RedirectToAction("GetUsers");
        }

        public ActionResult Edit(int id)
        {
            MongoServer objServer = MongoServer.Create("Server=localhost:27017");
            MongoDatabase objDatabse = objServer.GetDatabase("MVCTestDB1");
            IMongoQuery query = Query.EQ("ID", id);
            UserModel user = objDatabse.GetCollection<UserModel>("MVCTestDB1").Find(query).SingleOrDefault();
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(UserModel um)
        {
            MongoServer objServer = MongoServer.Create("Server=localhost:27017");
            MongoDatabase objDatabse = objServer.GetDatabase("MVCTestDB1");
            IMongoQuery query = Query.EQ("ID", um.ID);

            IMongoUpdate  updateQuery = Update.Set("UserName", um.UserName).Set("Password", um.Password).Set("Email", um.Email).Set("PhoneNo", um.PhoneNo).Set("Address", um.Address);
            UserModel user = objDatabse.GetCollection<UserModel>("Users").Find(query).SingleOrDefault();
            objDatabse.GetCollection<UserModel>("MVCTestDB1").Update(query, updateQuery);
            return RedirectToAction("GetUsers");
        }


    }
}
