namespace PortScanner
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Collections.Generic;
    using PcapDotNet.Core;
    using PcapDotNet.Packets;
    using PcapDotNet.Packets.Ethernet;
    using PcapDotNet.Packets.IpV4;
    using PcapDotNet.Packets.Transport;

    public class TcpScan : IScan
    {
        private readonly EthernetLayer _ethernetLayer;
        private readonly PacketCommunicator _packetCommunicator;
        private readonly Random _random;
        private const string _availableHost = "google.com";

        public TcpScan()
        {
            _ethernetLayer = new EthernetLayer();
            _random = new Random((int) DateTime.Now.Ticks);

            OpenEndpoints = new ObservableCollection<IP4EndPoint>();
            SendedEndpoints = new ObservableCollection<IP4EndPoint>();
            ResponsedEndpoints = new ObservableCollection<IP4EndPoint>();
            _responsedEndpointsDict = new Dictionary<int, IP4EndPoint>();

            var networkInterface = NetworkHelper.GetStandardInterface(_availableHost);
            _packetCommunicator = GetCommunicator(networkInterface);
            InitializeLayers(networkInterface);
            
        }
       
        public ObservableCollection<IP4EndPoint> OpenEndpoints { get; set; }
        public ObservableCollection<IP4EndPoint> SendedEndpoints { get; set; }
        public ObservableCollection<IP4EndPoint> ResponsedEndpoints { get; set; }
        private readonly Dictionary<int, IP4EndPoint> _responsedEndpointsDict;

        private ushort _sourcePort;
        private IpV4Address _sourceIP;
        private uint _sequenceNumber;

        public void SendRequestPacket(IpV4Address destinationIP, ushort destinationPort)
        {
            var layerIPv4 = new IpV4Layer
                                {
                                    Source = _sourceIP,
                                    Destination = destinationIP,
                                    Ttl = 64,
                                    Identification = 100,
                                    Protocol = IpV4Protocol.Tcp
                                };

            var layerTcp = new TcpLayer
                               {
                                   DestinationPort = destinationPort,
                                   SourcePort = _sourcePort,
                                   SequenceNumber = _sequenceNumber,
                                   ControlBits = TcpControlBits.Synchronize,
                                   Window = 65535
                               };

            var requestPacket = PacketBuilder.Build(DateTime.Now, _ethernetLayer, layerIPv4, layerTcp);
            _packetCommunicator.SendPacket(requestPacket);
            SendedEndpoints.Add(new IP4EndPoint(destinationIP, destinationPort));
        }

        public void HandleResponseLoop()
        {
            while (true)
            {
                Packet requestPacket;

                var result = _packetCommunicator.ReceivePacket(out requestPacket);
                if (result != PacketCommunicatorReceiveResult.Ok)
                    continue;
            
                var ipV4Datagram = requestPacket.Ethernet.IpV4;
                if (ipV4Datagram == null)
                    continue;
                var tcpDatagram = ipV4Datagram.Tcp;
                if (tcpDatagram == null)
                    continue;

                if (_sourcePort != tcpDatagram.DestinationPort)
                    continue;

                var key = (int) ipV4Datagram.Source.ToValue() + tcpDatagram.SourcePort;
                if (_responsedEndpointsDict.ContainsKey(key))
                    continue;
                
                var endpoint = new IP4EndPoint(ipV4Datagram.Source, tcpDatagram.SourcePort);
                _responsedEndpointsDict.Add(key, endpoint);
                ResponsedEndpoints.Add(endpoint);
            }
        }

        private static PacketCommunicator GetCommunicator(NetworkInterface networkInterface)
        {
            var livePacketDevice = LivePacketDevice.AllLocalMachine.
                First(x => x.Name.Remove(0, x.Name.IndexOf("{", StringComparison.Ordinal)) == networkInterface.Id);
            return livePacketDevice.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000);
        }

        private void InitializeLayers(NetworkInterface networkInterface)
        {
            var macAddresses = NetworkHelper.GetMacAddresses(networkInterface);
            _ethernetLayer.Source = new MacAddress(macAddresses[0]);
            _ethernetLayer.Destination = new MacAddress(macAddresses[1]);

            _sourceIP =
                new IpV4Address(
                    networkInterface.GetIPProperties().UnicastAddresses.First(
                        x => x.Address.AddressFamily == AddressFamily.InterNetwork).Address.ToString());
            _sourcePort = (ushort)_random.Next(1, 65535);
            _sequenceNumber = (uint)_random.Next();
        }
    }
}