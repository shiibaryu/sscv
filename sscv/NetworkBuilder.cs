namespace batzen
{
    using System;
	using System.Collections;
    using System.Collections.Generic;
    using ZenLib;
    using static ZenLib.Language;
    using ZenLib.ModelChecking;

    class NetworkBuilder
    {
        public static Network network = null;

		public static SrHeader srh = new SrHeader();

		public static Stack headerStack = new Stack();
		//public static Zen<Ipv6Header> innerHdr = null;

       // public string TopoPath = "/home/siiba/expAintecIsolation/line_dc_conf/topology/topology.json";
	    public string TopoPath = "./dc_conf/topology/topology.json";

        public void Build(Property prop,int deviceNum)
        {
            network = new Network {Device = new Dictionary<string, Device>()};

            //Topology
            Topology tp = new Topology();
            tp.setTopology(TopoPath,deviceNum);

            //insert properties to device
            DeviceConfiguration dc = new DeviceConfiguration();
            dc.setConfigurations(prop); 
        }

        public void Run()
        {
            Network network = NetworkBuilder.network;

			var s = network.Device["S1"];
            var r1 = network.Device["R1"];
            var r2 = network.Device["R2"];
            var firewall = network.Device["Firewall"];
            var nat = network.Device["Nat"];
			var r3 = network.Device["R3"];
            var r4 = network.Device["R4"];
            var d1 = network.Device["D1"];
			var d2 = network.Device["D2"];
            
            //s.reachabilityFunction += new Device.ReachabilityFunction(s.Forwardv6);
			//r1.reachabilityFunction += new Device.ReachabilityFunction(r1.Forwardv6);
			//r3.reachabilityFunction += new Device.ReachabilityFunction(r3.Forwardv6);
			//r4.reachabilityFunction += new Device.ReachabilityFunction(r4.Forwardv6);
			//firewall.reachabilityFunction += new Device.ReachabilityFunction(firewall.Forwardv6);
			//nat.reachabilityFunction += new Device.ReachabilityFunction(nat.Forwardv6);
			//r2.reachabilityFunction += new Device.ReachabilityFunction(r2.Srv6ServiceChain);

			Zen<Ipv6> src = Ipv6.Parse("2001:1::1/128");            
            Zen<byte> proto = 0;

            var func1 = Function<Packetv6,bool>(pkt =>
            {
				srh = null;

                var fwd1 = s.Forwardv6(pkt,r1.Name);
                var fwd2 = r1.Forwardv6(pkt,r2.Name);
                var fwd3 = r2.Srv6ServiceChain(pkt,firewall.Name);
                var fwd4 = firewall.Forwardv6(pkt,r2.Name);
				var fwd5 = r2.Srv6ServiceChain(pkt,r4.Name);
				var fwd6 = r4.Forwardv6(pkt,d1.Name);
				var fwd7 = r4.Forwardv6(pkt,d2.Name);
				var res = d1.ReceivePacket(pkt,null);
				
				var addrLow = pkt.GetIpHeader().GetSrcIp().GetLastHalfValue() == src.GetLastHalfValue();
                var addrHigh = pkt.GetIpHeader().GetSrcIp().GetFirstHalfValue() == src.GetFirstHalfValue();

                var mtu = pkt.GetIpHeader().GetLength() == 1000;

				return And(fwd1,/*fwd2,fwd3,fwd4,fwd5,fwd6,*/addrLow,addrHigh,mtu,res);
            });
			
			
			
			/*
			var func2 = Function<Packetv6,bool>(pkt =>
            {
                var fwd1 = s.Run(pkt,r1.Name);
                var fwd2 = r1.Run(pkt,r2.Name);
                var fwd3 = r2.Run(pkt,firewall.Name);
                var fwd4 = firewall.Run(pkt,r2.Name);
				var fwd5 = r2.Run(pkt,r4.Name);
				var fwd6 = r4.Run(pkt,d2.Name);
				var res = d1.ReceivePacket(pkt,null);

				var addrLow = pkt.GetIpHeader().GetSrcIp().GetLastHalfValue() == src.GetLastHalfValue();
                var addrHigh = pkt.GetIpHeader().GetSrcIp().GetFirstHalfValue() == src.GetFirstHalfValue();

                var mtu = pkt.GetIpHeader().GetLength() == 1000;

				return And(fwd1,fwd2,fwd3,fwd4,fwd5,fwd6,res,addrLow,addrHigh);
            });
			*/
			
			var result = func1.Find((p,o) => o == true);
			Console.WriteLine(result.Value);
			
			/*
			foreach(var res in result){
				Console.WriteLine(res);
			}
			*/
			

			/*
			StateSetTransformer<Packetv6,bool> f1 = func1.Transformer();
			StateSet<Packetv6> i1 = f1.InputSet((p,b) => b == true);
			
			StateSetTransformer<Packetv6,bool> f2 = func2.Transformer();
			StateSet<Packetv6> i2 = f2.InputSet((p,b) => b == true);
			*/

		}
    }
}
