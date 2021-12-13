namespace batzen
{
    using System.Collections.Generic;

    public class Interface
    {
        public string State {get;set;}

        public int Mtu {get;set;}

        public string Name { get; set; }

        public Device Owner { get; set; }

        public byte PortNumber { get; set; }

        public Interface Neighbor { get; set; }

        public string EtherAddr {get;set;}

        public string Ipv4Addr {get;set;}

        public List<string> gIpv6Addr {get;set;}

        public string hIpv6Addr {get;set;}

        public string loIpv6Addr {get;set;}

        public string liIpv6Addr {get;set;}

        //public Acl InboundAcl { get; set; }

        //public Acl OutboundACl { get; set; }

        public void setConfiguration(InterfaceProperty interfaceProperty)
        {
            Network network = NetworkBuilder.network;

            this.State = interfaceProperty.State;
            this.Mtu = interfaceProperty.Mtu;
            this.Name = interfaceProperty.Name;
            this.PortNumber = interfaceProperty.PortNumber;
            this.Owner = network.Device[interfaceProperty.Owner];
            this.EtherAddr = interfaceProperty.EtherAddr;
            this.Ipv4Addr = interfaceProperty.Ipv4Addr;
            this.gIpv6Addr = interfaceProperty.gIpv6Addr;
            this.hIpv6Addr = interfaceProperty.hIpv6Addr;
            this.loIpv6Addr = interfaceProperty.loIpv6Addr;
            this.liIpv6Addr = interfaceProperty.liIpv6Addr;
        }
    }
}
