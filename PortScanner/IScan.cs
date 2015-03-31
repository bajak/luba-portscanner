namespace PortScanner
{
    using System.Collections.ObjectModel;
    using PcapDotNet.Packets.IpV4;

    public interface IScan
    {
        void SendRequestPacket(IpV4Address destinationIP, ushort destinationPort);
        void HandleResponseLoop();
        ObservableCollection<IP4EndPoint> OpenEndpoints { get; set; }
        ObservableCollection<IP4EndPoint> SendedEndpoints { get; set; }
        ObservableCollection<IP4EndPoint> ResponsedEndpoints { get; set; }
    }
}
