using static System.Console;
using static Definitions.Default;
using System.Collections.Generic;

static class Client
{
    static void ProcessArgs(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
            switch (args[i])
            {
                case "-i":
                    RunCommand("connect");
                    break;
            }
    }
    static void TryRunCommand(string? cmd)
    {

        if (!RunCommand(cmd))
            Logger.Log("Undefined command. Try to write 'help'");
    }
    static void Main(string[] args)
    {
        Logger.Log("Created by Alan Wake 10/11/2022\n(C) - AWS Corporation. All rights reserved.\n");

        ProcessArgs(args);

        while (true)
        {
            TryRunCommand(ReadLine());
        }
    }
}