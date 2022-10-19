using static System.Text.Json.JsonSerializer;
using System.Collections.Generic;
using System.IO;

namespace Definitions
{
    public static class FactionHandler
    {
        public static string FactionPath = "";
        public static Dictionary<string, string>? NewTable
        {
            set
            {
                UpdateTable(Serialize(value));
            }
        }
        public static Dictionary<string,string>? GetLastTable()
        {
            return Deserialize<Dictionary<string, string>>(FileHandler.Read(Directory.GetCurrentDirectory()+FactionPath + "factions.json"));
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
        public static void Remove(string SteamID)
        {
            var tbl = GetLastTable();
            tbl?.Remove(SteamID);

            NewTable = tbl;
        }
        public static void Set(string SteamID,string faction)
        {
            var tbl = GetLastTable();
            tbl?.Add(SteamID, faction);

            NewTable = tbl;
        }
    }
}
