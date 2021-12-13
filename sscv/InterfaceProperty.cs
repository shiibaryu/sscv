namespace batzen
{
    using System.Collections.Generic;

    public class InterfaceProperty
    {
        public string State {get;set;}

        public int Mtu {get;set;}

        public string Name { get; set; }

        public string Owner { get; set; }

        public byte PortNumber { get; set; }

        public Interface Neighbor { get; set; }

        public string EtherAddr {get;set;}

        public string Ipv4Addr {get;set;}

        public List<string> gIpv6Addr = new List<string>();

        public string hIpv6Addr {get;set;}

        public string loIpv6Addr {get;set;}

        public string liIpv6Addr {get;set;}
    }
}