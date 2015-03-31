using System;
using PortScanner;

namespace UI
{
    internal class Output
    {
        public static void ShowInputInfo(Options options, long totalEndPoints)
        {
            Console.WriteLine("");
            Console.WriteLine("################################################");
            Console.WriteLine("Scanning from " + options.StartIP + " to " + options.EndIP);
            Console.WriteLine("Ports: " + options.Ports);
            Console.WriteLine("Total Endpoints: " + totalEndPoints);
            Console.WriteLine("Save to \"" + options.Filepath + "\"");
            Console.WriteLine("################################################");
            Console.WriteLine("");
        }

        public static void ShowEndPointInfo(string prefix, IP4EndPoint endPoint, int totalCount)
        {
            Console.WriteLine(prefix + endPoint.AddressStr + ":" + endPoint.Port + "\t=> " + totalCount);
        }

        public static void ShowError(string message)
        {
            Console.Write("\r\n\r\nERROR:\r\n" + message + "\r\n\r\n");
        }

        public static void ShowUsage()
        {
            Console.WriteLine("Usage: portscanner -s 46.163.69.0 -e 46.163.69.255 -p 21;8080 -f output.txt");
            Console.WriteLine("Type \"help\" for more infomation.");
        }

        public static void ShowSendIsDone()
        {
            Console.WriteLine("");
            Console.WriteLine("###############################################################");
            Console.WriteLine("Done sending packets. You can wait for the last responses.");
            Console.WriteLine("###############################################################");
            Console.WriteLine("");
        }
    }
}
