using MongoDB.Bson;
using MongoDB.Driver;
using SimpleBot.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleBot.Data
{
    public class MessageRepository
    {
        private MongoClient _client = ConnectionMongo.GetConnection();
        

        public MessageRepository()
        {
            
        }

        public void InsertUsuario(SimpleMessage message)
        {
            var db = _client.GetDatabase("databaseFiap");
            var col = db.GetCollection<BsonDocument>("schema01");

            var doc = new BsonDocument();
            doc.Add(nameof(message.Id), message.Id);
            doc.Add(nameof(message.User), message.User);
            doc.Add(nameof(message.Text), message.Text);

            col.InsertOne(doc);

            var usu = ManageProfileAccess(message);
        }

        public void InsertBot(string message)
        {
            var db = _client.GetDatabase("databaseFiap");
            var col = db.GetCollection<BsonDocument>("schema01");

            var doc = new BsonDocument();
            doc.Add("Bot", message);
            
            ///
            col.InsertOne(doc);
        }


        public async System.Threading.Tasks.Task<UserProfile> ManageProfileAccess(SimpleMessage message)
        {
            var db = _client.GetDatabase("databaseFiap");
            var col = db.GetCollection<BsonDocument>("schema02");

            var filtro = Builders<BsonDocument>.Filter.Eq("Id", message.Id);
            var res = col.Find(filtro).FirstOrDefault();

            var user = new UserProfile();
            user.Id = message.Id;
            
            if (res == null)
            {
                InsertUser(col, user);
            }
            else
            {
                filtro = await UpdateUser(message, col, filtro, res, user);
            }
            return user;
        }

        private static async System.Threading.Tasks.Task<FilterDefinition<BsonDocument>> UpdateUser(SimpleMessage message, IMongoCollection<BsonDocument> col, FilterDefinition<BsonDocument> filtro, BsonDocument res, UserProfile user)
        {
            user.Contador = (Int32)res.GetValue("Contador") + 1;
            filtro = await col.FindOneAndUpdateAsync(
                    Builders<BsonDocument>.Filter.Eq("Id", message.Id),
                    Builders<BsonDocument>.Update.Set("Contador", user.Contador)
                );
            return filtro;
        }

        private static void InsertUser(IMongoCollection<BsonDocument> col, UserProfile user)
        {
            user.Contador = 1;
            var doc = new BsonDocument();
            doc.Add(nameof(user.Id), user.Id);
            doc.Add(nameof(user.Contador), user.Contador);
            col.InsertOne(doc);
        }

        public int GetQtdeAcesso(string id)
        {
            var db = _client.GetDatabase("databaseFiap");
            var col = db.GetCollection<BsonDocument>("schema01");

            var filtro = Builders<BsonDocument>.Filter.Eq("Id", id);
            var res = col.Find(filtro).ToList();

            return res.Count+1;
        }
    }
}