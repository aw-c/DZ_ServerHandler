using WebSocketSharp;
using WebSocketSharp.Server;
using static System.Console;

static class Server
{
    class WSHandler : WebSocketBehavior
    {
        string? LastCommand;

        protected override void OnMessage(MessageEventArgs e)
        {
            WriteLine(e.Data);

        }
        protected override void OnOpen()
        {
            WriteLine($"{ID}'ve been connected");
        }
    }
    static void Main()
    {
        Definitions.Default.IsServer = true;

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