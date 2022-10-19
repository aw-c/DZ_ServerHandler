using static System.Console;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Definitions
{
    public static class Default
    {
        public class PrettyWS : WebSocketBehavior
        {
            public void PSend(string s)
            {
                Send(s);
            }
        }
        public static string[] StrArray(string? param1 = null, string? param2 = null,
            string? param3 = null, string? param4 = null)
        =>  new string[] { param1, param2, param3, param4 };
        public static bool IsServer = false;
        public static WebSocket? ws;
        public interface WIN_APP_Logger
        {
            public void Write(string s);
        }
        public static bool StringIsNull(string? s)
        {
            return s == "" || s == null;
        }
        public static Info? GetCommand(string name)
        {
            for (int i=0; i<Commands.Length; i++)
            {
                Info info = Commands[i];
                if (info.CommandName == name)
                    return info;
            }
            return null;
        }
        public static bool RunCommand(string cmd,string[]? args = null)
        {
            Info? t = GetCommand(cmd);

            if (t != null)
            {
                if (t.Act != null)
                    t.Act(t,args);
                return true;
            }

            return false;
        }
        public enum LOGGER_TYPE
        {
            CONSOLE,
            WIN_APP
        };
        public static class Logger
        {
            public static LOGGER_TYPE LOGGER_TYPE = LOGGER_TYPE.CONSOLE;
            public static WIN_APP_Logger? LOGGER_OUTPUT;
            public static void Log(string s,bool optNewLine = true)
            {
                char n = optNewLine ? '\n' : '\0';
                s = $"{s}{n}";
                switch (LOGGER_TYPE)
                {
                    case LOGGER_TYPE.CONSOLE:
                        Write(s);
                        break;
                    case LOGGER_TYPE.WIN_APP:
                        LOGGER_OUTPUT?.Write(s);
                        break;
                }
            }
        }
        public class Info
        {
            public Action<Info,string[]?> Act;
            public Action<PrettyWS, string[]?>? ServerAction;
            public string CommandName;
            public string Description;

            public Info(string CmdName, Action<Info,string[]?> act, string desc, Action<PrettyWS, string[]?>? serveract = null)
            {
                CommandName = CmdName;

                Act = act;
                Description = desc;
                ServerAction = serveract;
            }
        }
        public static Info DeclareCommand(string CmdName, Action<Info,string[]?>? act, string desc, 
            Action<PrettyWS, string[]?>? serveract = null) 
            => new Info(CmdName,act, desc, serveract);
        public static Info[] Commands = new Info[]
        {
            DeclareCommand("help",(info,args) =>
                {
                    Logger.Log("\nAvailable commands: ");
                    /*foreach(KeyValuePair<string, Info> kvp in Commands)
                    {
                        Logger.Log($"'{kvp.Key}' {kvp.Value.Description} ");
                    }*/
                    for (int i = 0; i < Commands.Length; i++)
                    {
                        Info _info = Commands[i];
                        Logger.Log($"'{_info.CommandName}' {_info.Description}");
                    }
                },"Command to get all commands"),
            DeclareCommand("connect",(info,args) =>
                {
                    string? IP = args?[0];
                    if (IP == null)
                    {
                        Logger.Log("Enter the IP address: ",false);
                        IP = ReadLine();
                    }

                    WriteLine($"Connecting to {IP}...");

                    try
                    {
                        ws = new WebSocket($"ws://{IP}/handler");

                        /*ws.Log.Level = WebSocketSharp.LogLevel.Fatal;*/

                        ws.OnOpen += (sender,data) =>
                        {
                            Logger.Log($"Connected to {IP} succesfully");
                        };

                        ws.OnMessage += (sender, e) =>
                        {
                            Logger.Log(e.Data);
                        };
                        ws.Connect();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Unnable to connect to the specified IP address");
                    }
                },"Attempt to connect to the server with specified IP address"),
            DeclareCommand("faction_get",(info,args) =>
            {
                var tbl = StrArray(info.CommandName,args?[0]);
                if (tbl[1] == null)
                {
                    Logger.Log("Enter SteamID: ",false);
                    tbl[1] = ReadLine();
                }

                ws?.Send(ArgsHandler.GetPerfectArgs(tbl));
            }
            ,"Command to get faction of the specified SteamID",(ws,args) =>
            {
                ws.PSend(FactionHandler.GetFactionBySteamID(args[1]));
            }),
            DeclareCommand("faction_get_all",null,"Command to get all members and their membership in the factions"),
            DeclareCommand("faction_add",(info,args) => 
            {
                var tbl = StrArray(info.CommandName,args?[0],args?[1]);
                if (tbl[1] == null)
                {
                    Logger.Log("Enter SteamID: ",false);
                    tbl[1] = ReadLine();

                    Logger.Log("Enter faction: ",false);
                    tbl[2] = ReadLine();
                }

                ws?.Send(ArgsHandler.GetPerfectArgs(tbl));
            },
            "Command to add to the faction spawn specified SteamID",(ws,args) =>
            {
                FactionHandler.Add(args[1],args[2]);
            }),
            DeclareCommand("faction_remove",(info,args) =>
            {
                var tbl = StrArray(info.CommandName,args?[0]);
                if (tbl[1] == null)
                {
                    Logger.Log("Enter SteamID: ",false);
                    tbl[1] = ReadLine();
                }
                
                ws?.Send(ArgsHandler.GetPerfectArgs(tbl));
            },
            "Command to remove from the faction spawn specified SteamID",(ws,args) =>
            {
                FactionHandler.Remove(args[1]);
            }),
            DeclareCommand("file_flush",null,"Command to write data to the specified file"),
            DeclareCommand("file_read",null,"Command to read data from the specified file"),

        };
    }
}