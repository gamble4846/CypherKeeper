using CypherKeeper.AuthLayer.Models;
using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.DataAccess.MongoDB.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MongoDB.Bson.IO;
using MongoDB.Driver;

namespace CypherKeeper.DataAccess.MongoDB.Impl
{
    public class AdminDataAccess : IAdminDataAccess
    {
        private CommonFunctions _CF { get; set; }
        private MongoDBValues MongoValues { get; set; }
        private IMongoDatabase Database { get; set; }
        private MongoClient Client { get; set; }
        private MongoClientSettings Settings { get; set; }

        public AdminDataAccess(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
        {
            _CF = new CommonFunctions(configuration, httpContextAccessor);
            MongoValues = _CF.GetMongoDBValues();

            Settings = MongoClientSettings.FromConnectionString(MongoValues.ConnectionString);
            Settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            Client = new MongoClient(Settings);
            Database = Client.GetDatabase(MongoValues.Database);
        }

        public tbAccessModel Register(tbAccessModel model)
        {
            var Collection = "tbAccess";
            var collection = Database.GetCollection<tbAccessModel>(Collection);
            collection.InsertOne(model);
            return model;
        }

        public tbAccessModel GetByEmail(string email)
        {
            var Collection = "tbAccess";
            var collection = Database.GetCollection<tbAccessModel>(Collection);
            var filter = Builders<tbAccessModel>.Filter.Eq("Email", email);
            var documents = collection.Find(filter).ToList();

            if(documents.Count == 0)
            {
                return null;
            }
            else
            {
                return documents[0];
            }
        }

        public tbAccessModel GetByUsername(string username)
        {
            var Collection = "tbAccess";
            var collection = Database.GetCollection<tbAccessModel>(Collection);
            var filter = Builders<tbAccessModel>.Filter.Eq("Username", username);
            var documents = collection.Find(filter).ToList();

            if (documents.Count == 0)
            {
                return null;
            }
            else
            {
                return documents[0];
            }
        }

        public long UpdateSettings(SettingsModel model, tbAccessModel CurrentUser)
        {
            var Collection = "tbAccess";
            var collection = Database.GetCollection<tbAccessModel>(Collection);
            var filter = Builders<tbAccessModel>.Filter.Eq("_id", CurrentUser._id);
            var update = Builders<tbAccessModel>.Update.Set("Settings", model);
            var updateResult = collection.UpdateOne(filter, update);
            return updateResult.ModifiedCount;
        }
    }
}
