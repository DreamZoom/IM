using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Core
{
    public class Message
    {
        public string Sender { get; set; }

        public string Reciver { get; set; }

        public string Content { get; set; }

        public MessageType MsgType { get; set; }

        public string ContentType { get; set; }

        
    }
}
