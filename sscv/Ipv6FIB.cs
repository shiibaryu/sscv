namespace batzen
{
    using System;
    using System.IO;
    using batzenLib;

    public class Ipv6ForwardingInfo
    {
        public string Prefix;

        public string NextHop;

        public string Interface;
    }

    public class Ipv6FIB
    {
        public v6RadixTree Radix;
    }
}