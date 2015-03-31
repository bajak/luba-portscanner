namespace PortScanner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;

    class NetworkHelper
    {
        private static string RequestMacAddress(IPAddress ipAddress)
        {
            var macAddressBytes = new byte[6];
            var length = new IntPtr(macAddressBytes.Length);
            NativeMethods.SendARP(new IntPtr(BitConverter.ToInt32(ipAddress.GetAddressBytes(), 0)), IntPtr.Zero, macAddressBytes,
                    ref length);
            var macAddress = BitConverter.ToString(macAddressBytes, 0, length.ToInt32());
            return macAddress;
        }

        public static NetworkInterface GetStandardInterface(string availableHost)
        {
            var ipHostEntry = Dns.GetHostEntry(availableHost);
            var byteArray1 = IPAddress.Parse(ipHostEntry.AddressList[0].ToString()).GetAddressBytes();
            var byteArray2 = new byte[4];
            byteArray1[0] = byteArray2[3];
            byteArray1[1] = byteArray2[2];
            byteArray1[2] = byteArray2[1];
            byteArray1[3] = byteArray2[0];
            IntPtr interfaceIndex;
            NativeMethods.GetBestInterface(new IntPtr(BitConverter.ToUInt32(byteArray1, 0)), out interfaceIndex);
            return  NetworkInterface.GetAllNetworkInterfaces().First(
                        x => x.GetIPProperties().GetIPv4Properties().Index == interfaceIndex.ToInt32());
        }

        public static string[] GetMacAddresses(NetworkInterface networkInterface)
        {
            var macAddresses = new List<string>
                                   {
                                       BitConverter.ToString(networkInterface.GetPhysicalAddress().GetAddressBytes()).
                                           Replace(
                                               "-", ":")
                                   };
            macAddresses.AddRange(
                networkInterface.GetIPProperties().GatewayAddresses.Select(
                    gatewayAddress => RequestMacAddress(gatewayAddress.Address).Replace("-", ":")));

            return macAddresses.ToArray();
        }
    }
}
