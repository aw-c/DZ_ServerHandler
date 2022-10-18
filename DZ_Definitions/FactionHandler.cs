using static System.Text.Json.JsonSerializer;
using System.Collections.Generic;

namespace Definitions
{
    public static class FactionHandler
    {
        public static string FactionPath = "";
        public static Dictionary<string,string>? GetLastTable()
        {
            return Deserialize<Dictionary<string, string>>(FileHandler.Read(FactionPath));
        }
        public static void UpdateTable(string json)
        {
            FileHandler.Write(FactionPath, json);
        }
        public static string? GetFactionBySteamID(string SteamID)
        {
            return GetLastTable()?[SteamID];
        }
        public static string GetAll()
        {
            var tbl = GetLastTable();
            string s = "";

            if (tbl != null)
                foreach(KeyValuePair<string,string> kvp in GetLastTable())
                    s += $"{kvp.Key}    =    {kvp.Value}\n";

            return s;
        }
    }
}
