using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace CypherKeeper.Utility
{
    public class MongoLayer
    {
        public static MongoClient GetMongoClient()
        {
            const string connectionUri = "mongodb+srv://sa:18158114@cypherkeeper.c532qvf.mongodb.net/?retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            return client;
        }

        public static T InsertMongo<T>(T model, string Database, string Collection)
        {
            var client = GetMongoClient();
            var database = client.GetDatabase(Database);
            var collection = database.GetCollection<T>(Collection);
            collection.InsertOne(model);
            return model;
        }
    }
}
