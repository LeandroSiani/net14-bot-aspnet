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

            var usu = GetProfile(message);
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


        public UserProfile GetProfile(SimpleMessage message)
        {
            var db = _client.GetDatabase("databaseFiap");
            var col = db.GetCollection<BsonDocument>("schema01");

            var filtro = Builders<BsonDocument>.Filter.Eq("Id", message.Id);
            var res = col.Find(filtro).FirstOrDefault();

            var user = new UserProfile();
            user.Id = message.Id;
            if (res.Count() == 0)
            {
                user.Contador = 1;
                var doc = new BsonDocument();
                doc.Add(nameof(message.Id), message.Id);
                doc.Add(nameof(user.Contador), user.Contador);
                doc.Add(nameof(message.User), message.User);
                doc.Add(nameof(message.Text), message.Text);

                col.InsertOne(doc);
            }
            else
            {
                try
                {
                    user.Contador = res.GetValue("Contador").AsInt32 + 1;

                }
                catch
                {
                    user.Contador = 1;

                }
                var update = Builders<BsonDocument>.Update.Set("Contador", user.Contador);
                col.FindOneAndUpdateAsync(filtro, update);
            }
            return user;
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