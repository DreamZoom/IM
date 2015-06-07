using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace IM.Core
{
    public class MessageHelper
    {
        public static string Json(Message message)
        {
            JsonSerializer js = new JsonSerializer();
            StringWriter str=new StringWriter();
            js.Serialize(str, message);
            return str.ToString();
        }

        public static Message Object(string jsonString)
        {
            JsonSerializer js = new JsonSerializer();
            var jtr = new JsonTextReader(new StringReader(jsonString));
            return js.Deserialize<Message>(jtr);
        }
    }
}
