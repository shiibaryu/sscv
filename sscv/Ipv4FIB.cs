namespace batzen
{
    using System;
    using System.IO;
    using batzenLib;

    public class Ipv4ForwardingInfo
    {
        public string Prefix;

        public string NextHop;

        public string Interface;
    }

    public class Ipv4FIB
    {
        public v4RadixTree Radix;
    }
}