using System;
using System.Collections.Generic;

namespace Botler.Commands.Core.Seen
{
    class set
    {
        public static void go(string Nick, string Channel, string Message)
        {
            DateTime timeNow = DateTime.Now;
            person newPerson = new person(Nick.ToLower(), Channel, Message, timeNow.ToString());
            List<person> response = Program.GlobalVar.seenList.FindAll(x => x.nick == Nick.ToLower());
            foreach (person p in response)
            {
                if (response != null && (p.nick == newPerson.nick && p.channel == newPerson.channel))
                {
                    Program.GlobalVar.seenList.Remove(p);
                }
            }
            Program.GlobalVar.seenList.Add(newPerson);
        }
    }
}
