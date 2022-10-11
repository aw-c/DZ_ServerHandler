using static System.Console;
using WebSocketSharp;

namespace Definitions
{
    public static class Default
    {
        public static WebSocket? ws;
        public interface WIN_APP_Logger
        {
            public void Write(string s);
        }
        public static bool RunCommand(string cmd)
        {
            Info? t;
            Commands.TryGetValue(cmd,out t);

            if (t != null)
            {
                if (t.Act != null)
                    t.Act();
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
            public Action Act;
            public Action ServerAction;
            public string Description;

            public Info(Action act, string desc)
            {
                Act = act;
                Description = desc;
            }
        }
        public static Info DeclareCommand(Action? act, string desc) => new Info(act, desc);
        public static Dictionary<string, Info> Commands = new Dictionary<string, Info>()
        {
            {
                "help", DeclareCommand(() =>
                {
                    WriteLine("\nAvailable commands: ");
                    foreach(KeyValuePair<string, Info> kvp in Commands)
                    {
                        Logger.Log($"'{kvp.Key}' {kvp.Value.Description} ");
                    }
                },"Command to get all commands")
            },
            {
                "connect",DeclareCommand(() =>
                {
                    Write("Enter the IP address: ");
                    string? IP = ReadLine();

                    WriteLine($"Connecting to {IP}...");

                    try
                    {
                        ws = new WebSocket($"ws://{IP}/handler");
                        
                        ws.Log.Level = WebSocketSharp.LogLevel.Fatal;

                        ws.OnOpen += (sender,data) =>
                        {
                            WriteLine($"Connected to {IP} succesfully");
                        };

                        ws.OnMessage += (sender, data) =>
                        {
                            WriteLine(data);
                        };
                        ws.Connect();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Unnable to connect to the specified IP address");
                    }
                },"Attempt to connect to the server with specified IP address")
            },
            {
                "faction_get",DeclareCommand(null,"Command to get faction of the specified SteamID")
            },
            {
                "faction_get_all",DeclareCommand(null,"Command to get all members and their membership in the factions")
            },
            {
                "faction_add",DeclareCommand(null,"Command to add to the faction spawn specified SteamID")
            },
            {
                "faction_remove",DeclareCommand(null,"Command to remove from the faction spawn specified SteamID")
            },
            {
                "file_flush",DeclareCommand(null,"Command to write data to the specified file")
            },
            {
                "file_read",DeclareCommand(null,"Command to read data from the specified file")
            },
        };
    }
}