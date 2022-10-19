using static Definitions.Default;
namespace Definitions
{
    public static class ArgsHandler
    {
        public static string GetPerfectArgs(string[] args)
        {
            string tosend = "";

            for (int i = 0; i < args.Length; i++)
            {
                var s = args[i];
                if (!StringIsNull(s))
                    if (tosend == "")
                        tosend = s;
                    else
                        tosend = $"{tosend} {s}";
            }

            return tosend;

            /*ws.Send(tosend);*/
        }
        public static string[] GetPerfectArgs(string args)
        {
            return args.Split(' ');
        }
    }
}
