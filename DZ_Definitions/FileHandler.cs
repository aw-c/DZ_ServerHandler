namespace Definitions
{
    public static class FileHandler
    {
        /*public class Info
        {
            public bool? Succ;
            public string? Returns;
        }*/

        public static string Read(string path)
        {
            return File.ReadAllText(path);
        }

        public static void Write(string path,string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
