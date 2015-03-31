namespace PortScanner
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net;

    public class IPRange
    {
        public IPRange(IPAddress startIP, IPAddress endIP)
        {
            StartIP = startIP;
            EndIP = endIP;
        }

        public IPRange(string startIP, string endIP)
        {
            StartIP = IPAddress.Parse(startIP);
            EndIP = IPAddress.Parse(endIP);
        }

        public IPAddress StartIP { get; private set; }
        public IPAddress EndIP { get; private set; }

        public IEnumerable<string> GetRange()
        {
            var sIP = IPToUint(StartIP.GetAddressBytes());
            var eIP = IPToUint(EndIP.GetAddressBytes());
            while (sIP < eIP)
            {
                yield return new IPAddress(ReverseBytesArray(sIP)).ToString();
                sIP++;
            }
        }

        protected static uint ReverseBytesArray(uint ip)
        {
            var bytes = BitConverter.GetBytes(ip);
            Array.Reverse(bytes);
            return (uint)BitConverter.ToInt32(bytes, 0);
        }
        
        protected uint IPToUint(byte[] ipBytes)
        {
            var bConvert = new ByteConverter();
            uint ipUint = 0;
            
            var shift = 24;

            foreach (var b in ipBytes)
            {
                if (ipUint == 0)
                {
                    var convertTo = bConvert.ConvertTo(b, typeof (uint));
                    if (convertTo != null)
                        ipUint = (uint)convertTo << shift;
                    shift -= 8;
                    continue;
                }

                if (shift >= 8)
                {
                    var convertTo = bConvert.ConvertTo(b, typeof (uint));
                    if (convertTo != null)
                        ipUint += (uint)convertTo << shift;
                }
                else
                {
                    var to = bConvert.ConvertTo(b, typeof (uint));
                    if (to != null)
                        ipUint += (uint)to;
                }

                shift -= 8;
            }

            return ipUint;
        }
    }
}
