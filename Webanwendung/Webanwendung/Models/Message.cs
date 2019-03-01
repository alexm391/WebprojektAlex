using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webanwendung.Models
{
    public class Message
    {
        public string Header { get; set; }
        public string MessageText { get; set; }
        public string Solution { get; set; }

        public Message() : this("", "", "") { }
        public Message(string header, string messageText)
        {
            this.Header = header;
            this.MessageText = messageText;
        }

        public Message(string header, string messageText, string solution)
        {
            this.Header = header;
            this.MessageText = messageText;
            this.Solution = solution;
        }

    }
}