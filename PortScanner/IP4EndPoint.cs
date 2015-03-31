namespace PortScanner
{
    using PcapDotNet.Packets.IpV4;

    public class IP4EndPoint
    {
        public IP4EndPoint(IpV4Address address, ushort port)
        {
            Address = address;
            Port = port;
        }

        public string AddressStr
        {
            get { return Address.ToString(); }
        }
        public IpV4Address Address { get; private set; }
        public ushort Port { get; private set; }
    }
}
