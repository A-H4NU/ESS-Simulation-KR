using System;
using System.IO;
using System.Text;

namespace ESS_Simulation
{
    internal static class ToFile
    {
        private static StringBuilder _sb = new StringBuilder();

        public static void Write(string message) => _sb.Append(message);

        public static void WriteLine(string message) => Write(message + Environment.NewLine);

        public static void WriteLine() => Write(Environment.NewLine);

        public static void Save()
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ESS SIMULATION");
            string path = Path.Combine(dir, $"ESS SIMULATION {DateTime.Now.ToString("yy-MM-dd-HH-mm-ss")}.log");
            string message = _sb.ToString();
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.WriteAllText(path, message);
            Console.Write(message);
            _sb = new StringBuilder();
        }
    }
}
