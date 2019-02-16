using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleBot.Logic
{
    public class SimpleMessage
    {
        public string Id { get; }
        public string User { get; }
        public string Text { get; }
        public List<string> Interests
        {
            get;
        }

        public SimpleMessage(string id, string username, string text, List<string> interests)
        {
            this.Id = id;
            this.User = username;
            this.Text = text;
            this.Interests = interests;
        }
    }
}