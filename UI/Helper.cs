using System.IO;
using System.Reflection;

namespace UI
{
    public class Helper
    {
        public static void WriteToFile(string fileName, string ip, string port)
        {
            try
            {
                if (!fileName.Contains(@"\"))
                    fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + fileName;
                var streamWriter = new StreamWriter(fileName, true);
                streamWriter.WriteLine(ip + ":" + port);
                streamWriter.Close();
            }

            catch (IOException e)
            {
                Output.ShowError(e.Message);
            }
        }
    }
}
