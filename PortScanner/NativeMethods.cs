namespace PortScanner
{
    using System;
    using System.Runtime.InteropServices;

    public static class NativeMethods
    {
        [DllImport("iphlpapi.dll")]
        internal static extern IntPtr SendARP(IntPtr destIP, IntPtr srcIP, [Out] byte[] pMacAddr, ref IntPtr phyAddrLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static extern IntPtr GetBestInterface(IntPtr destAddr, out IntPtr bestIfIndex);
    }
}