using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Definitions
{
    public static class FileHandler
    {
        public class Info
        {
            public bool? Succ;
            public string? Returns;
        }

        public static string Read(string path)
        {
            return "";
        }
        public static void Write(string path,string content)
        {
            FileStream fs = new FileStream("dsa",FileMode.Open)

            if (fs.CanWrite)
                fs.Write(byte.Parse(content));
        }
    }
    public static class FactionHandler
    {
        public static string GetFactionBySteamID(string SteamID)
        {
            return "";
        }
    }
}
