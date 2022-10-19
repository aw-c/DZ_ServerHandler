using WebSocketSharp;
using WebSocketSharp.Server;
using static System.Console;
using static System.Text.Json.JsonSerializer;
using Definitions;

static class Server
{
    static void ProcessArgs(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
            switch (args[i])
            {
                case "-factionPath":
                    FactionHandler.FactionPath = args[i + 1];
                    break;
                case "-allowedKeys":
                    Default.AllowedKeys = Deserialize<Dictionary<string, string>>
                        (FileHandler.Read(Default.CurrentFolder + args[i + 1]));
                    break;
            }
    }
    public class WSHandler : Default.PrettyWS
    {
        /*public string? LastCommand;*/


        protected override void OnMessage(MessageEventArgs e)
        {
            string argss = e.Data;
            string[] args = ArgsHandler.GetPerfectArgs(argss);

            Default.Logger.Log($"{Name}'ve been sent '{argss}' #{argss.Length},{args.Length}");

            if (!IsAuthorized)
                if (args[0] != "auth")
                    return;

            Send($"Server received the '{args[0]}' command");

            try
            {
                Default.Info info = Default.GetCommand(args[0]);
                info.ServerAction(this,args);

                Default.Logger.Log($"{Name} command: '{args[0]}' have been runed");
            }
            catch (Exception ex)
            {
                Default.Logger.Log($"On occurred error: {ex.Message}\n" +
                    $"{Name} command: '{args[0]}' haven't been runed");
                Send($"Command '{args[0]}' didn't run. Arguments or elements are invalid.");
            }
        }
        protected override void OnOpen()
        {
            WriteLine($"{Name}'ve been connected");
        }
    }
    static void Main(string[] args)
    {
        Default.IsServer = true;
        Default.CurrentFolder = Directory.GetCurrentDirectory();

        ProcessArgs(args);

        while (true)
        {
            var wssv = new WebSocketServer("ws://127.0.0.1:3000");

            wssv.AddWebSocketService<WSHandler>("/handler");

            wssv.Start();

            WriteLine($"Server've been stated at {DateTime.Now}");
            WriteLine($"Server's IP: {wssv.Address}:{wssv.Port}");

            ReadLine();

            WriteLine("Rebooting the server\n");
            wssv.Stop();
        }
    }
}