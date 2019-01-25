using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleBot.Data
{
    public class ConnectionMongo
    {
        private static MongoClient _client;

        public ConnectionMongo()
        {
            
        }

        public static MongoClient GetConnection()
        {
           if (_client == null)
            {
                _client = new MongoClient("mongodb://localhost:27017");
            }
            return _client;
        }
    }
}