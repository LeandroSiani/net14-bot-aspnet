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
        }

        public void InsertBot(string message)
        {
            var db = _client.GetDatabase("databaseFiap");
            var col = db.GetCollection<BsonDocument>("schema01");

            var doc = new BsonDocument();
            doc.Add("Bot", message);
            
            col.InsertOne(doc);
        }
    }
}