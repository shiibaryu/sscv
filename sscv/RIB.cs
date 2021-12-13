namespace batzen
{
    using System;
    using System.IO;
    using Deedle;
    using batzenLib;

    public class RTable
    {
        public string Network;

        //nexthop devicename
        public string NextHop;

        //nexthop interfacename
        public string NextHopIf;

        public string SrcInterface;

        public string SrcIpAddress;

        public string Protocol;

        public int Metric;

        public int AdminDistance;

        public string Vrf;
    }

    public class RIB
    {
        //public RadixTree rt;
    }
}