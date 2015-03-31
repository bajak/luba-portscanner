using System;

namespace PortScanner
{
    using System.Net.Sockets;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using PcapDotNet.Packets.IpV4;
    using System.Linq;

    public class Scanner
    {
        public Scanner(IScan scan, IPRange[] ipRanges, int[] ports)
        {
            IPRanges = ipRanges;
            Ports = ports;
            Scan = scan;

            foreach (var ipRange in IPRanges)
                TotalEndPoints += ipRange.GetRange().Count() * ports.Length;
        }

        public IPRange[] IPRanges { get; private set; }
        public int[] Ports { get; private set; }
        public IScan Scan { get; private set; }
        public long TotalEndPoints { get; private set; }

        public void Start()
        {
            var tcpRequestLoop = new Thread(Scan.HandleResponseLoop);
            tcpRequestLoop.Start();
            foreach (var port in Ports)
            {
                foreach (var ipRange in IPRanges)
                {
                    var tmpPort = port;
                    Parallel.ForEach(ipRange.GetRange(), x => PortScan(x, tmpPort));
                }
            }
        }

        private void PortScan(string ipAddress, int port)
        {
            Scan.SendRequestPacket(new IpV4Address(ipAddress), (ushort)port);
        }
    }
}
