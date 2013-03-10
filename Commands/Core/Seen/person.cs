using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botler.Commands.Core.Seen
{
    class person
    {
        public string nick;
        public string channel;
        public string message;
        public string time;

        public person(string inNick, string inChannel, string inMessage, string inTime)
        {
            nick = inNick;
            channel = inChannel;
            message = inMessage;
            time = inTime;
        }
    }
}
