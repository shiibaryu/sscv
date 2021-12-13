namespace batzen
{
    using System;
    using System.Net;
	using System.Collections;
    using ZenLib;
    using static ZenLib.Language;
    using System.Collections.Generic;

    public class Device
    {
        public bool isVisited = false;

        public string Name { get; set; }

        public Dictionary<string,Interface> Interface {get;set;}

        public ForwardingTable FwTables {get;set;}

        public ForwardingTablev6 v6FwTables {get;set;}

        public Ipv4FIB v4Fib {get;set;}

        public Ipv6FIB v6Fib {get;set;}

        public Dictionary<string,string> Acl {get;set;}

        public List<Srv6Rule> Srv6Rule {get;set;}

        public delegate Zen<bool> ReachabilityFunction(Zen<Packetv6> pkt,string neiborDevice);
        public ReachabilityFunction reachabilityFunction;

        public void setConfiguration(Device device,DeviceProperty deviceProperty)
        {
            Interface tmp = null;

            //for Interface
            foreach(InterfaceProperty interfaceProperty in deviceProperty.ifp.Values){
                string InterfaceName = interfaceProperty.Name;

                if(device.Interface.TryGetValue(InterfaceName,out tmp) == true){
                    Interface Interface = device.Interface[InterfaceName];
                    Interface.setConfiguration(interfaceProperty);
                }
                else{
                    Interface newInterface = new Interface();
                    newInterface.setConfiguration(interfaceProperty);

                    device.Interface[InterfaceName] = newInterface;
                }
           }

           //for Route
           int num = deviceProperty.routepv6.Count;
           device.v6FwTables = new ForwardingTablev6();
           device.v6FwTables.Rules = new ForwardingRulev6[num];

           for(int i=0;i<num;i++){
               device.v6FwTables.setConfiguration(i,deviceProperty.routepv6[i],device.Interface);
           }

           device.Srv6Rule = deviceProperty.Srv6Rule;
        }

        public Zen<bool> Forwardv4(Zen<Packet> pkt,string neiborDevice)
        {
            return rForwardv4(pkt,this.FwTables.Rules,neiborDevice,0);
        }

        public Zen<bool> rForwardv4(Zen<Packet> packet,ForwardingRule[] rules,string neiborDevice,int i)
        {
            if(i == rules.Length){
                return Constant<bool>(false);
            }

            var rule = rules[i];

            return If(
                And(rule.Matches(packet.GetIpHeader()),rule.Interface.Neighbor.Owner.Name == neiborDevice,rule.Interface.State == "UP"),
                true,
                rForwardv4(packet,rules,neiborDevice,i + 1)
            );
        }

        public Zen<bool> Forwardv6(Zen<Packetv6> pkt,string neiborDevice)
        {
			//Console.WriteLine(this.Name + ":" + neiborDevice);
            return rForwardv6(pkt.GetIpHeader(),this.v6FwTables.Rules,neiborDevice,0);
            
            //return rForwardv6(pkt.GetIpHeader(),this.v6FwTables.Rules,neiborDevice,0);
        }

        public Zen<bool> SwitchForwardv6(Device sw,string neiborDevice)
        {
            Zen<bool> tmp = false;
            Zen<bool> result = false;
           
            foreach(var inf in sw.Interface){
                    Interface cur_inf = inf.Value;
                    tmp = And(cur_inf.Neighbor.Owner.Name == neiborDevice,cur_inf.State == "UP");
                    If(tmp == true,result = true,tmp = false);
            }
            
            return result;
        }

        // for recursive
        public Zen<bool> rForwardv6(Zen<Ipv6Header> v6hdr,ForwardingRulev6[] rules,string neiborDevice,int i)
        {
            if(i == rules.Length){
                return false;
            }

            var rule = rules[i];

			if(rule.DstIpHigh.firstHalfValue == 0 && rule.DstIpHigh.lastHalfValue == 0 && rule.Interface.Neighbor.Owner.Name == neiborDevice){
				return true;
			}
       
	   		/*
            if(rule.Interface.Neighbor != null && rule.Interface.Neighbor.Owner.Name.Contains("SW")){
                return If(And(rule.Matches(v6hdr),rule.Interface.State == "UP",v6hdr.GetLength() <= rule.Interface.Mtu,SwitchForwardv6(rule.Interface.Neighbor.Owner,neiborDevice)),
                        true,
                        rForwardv6(v6hdr,rules,neiborDevice,i + 1)
                    );
            }*/
            
            return If(
                And(rule.Matches(v6hdr),rule.Interface.Neighbor.Owner.Name == neiborDevice,rule.Interface.State == "UP",v6hdr.GetLength() <= rule.Interface.Mtu),
                true,
                rForwardv6(v6hdr,rules,neiborDevice,i + 1)
            );
        }

        public Zen<Ipv6Header>[] v6AddrsToUlong(List<string> gIpv6Addr,Zen<Ipv6> src)
        {
            int size = gIpv6Addr.Count;
            Zen<Ipv6Header>[] v6hdr = new Zen<Ipv6Header>[size];

            for(int i=0;i<size;i++){
                string[] str = gIpv6Addr[i].Split("/");
                string infAddr = str[0];
                infAddr += "/128";
                v6hdr[i] = Ipv6Header.Create(Ipv6.Parse(infAddr),src,0);        
            }
            
            return v6hdr;
        }

        public Zen<bool> ReceivePacket(Zen<Packetv6> pkt,string neiborDevice)
        {
            Zen<bool> truth = false;
           
            foreach(KeyValuePair<string,Interface> inf in this.Interface)
            {
                Interface devInf = inf.Value;    
                Zen<Ipv6Header>[] infAddr = v6AddrsToUlong(devInf.gIpv6Addr,pkt.GetIpHeader().GetSrcIp());
                int len = infAddr.Length;

                for(int i=0;i<len;i++){
                    Zen<bool> dstFirstHalfMatch = pkt.GetIpHeader().GetDstIp().GetFirstHalfValue() == infAddr[i].GetDstIp().GetFirstHalfValue();
                    Zen<bool> dstLastHalfMatch = pkt.GetIpHeader().GetDstIp().GetLastHalfValue() == infAddr[i].GetDstIp().GetLastHalfValue();

                    truth = If(truth,truth = true,If(And(dstFirstHalfMatch,dstLastHalfMatch),truth = true,truth = false));
           
                }
            }

            return truth;
        }

        public Zen<bool> SegmentMatches(Zen<Packetv6> packet, Srv6Rule rule)
        {
            Zen<Ipv6> v6Addr = packet.GetIpHeader().GetDstIp();
            Zen<bool> firstHaflMatch = null;
            Zen<bool> lastHaflMatch = null;
			
            if(rule.prefix == 128){
                firstHaflMatch = v6Addr.GetFirstHalfValue() == rule.targetAddress.firstHalfValue;
                lastHaflMatch = v6Addr.GetLastHalfValue() == rule.targetAddress.lastHalfValue;
                return And(firstHaflMatch,lastHaflMatch);
            }
            else if(rule.prefix == 64){
                return firstHaflMatch = v6Addr.GetFirstHalfValue() == rule.targetAddress.firstHalfValue;
            }
            else if(rule.prefix == 96){
                firstHaflMatch = v6Addr.GetFirstHalfValue() == rule.targetAddress.firstHalfValue;   

                Zen<ulong> left = 0xFFFFFFFF00000000;
                Zen<ulong> symAddrLastHalfValue = v6Addr.GetLastHalfValue() & left;
                ulong ruleLeft = rule.targetAddress.lastHalfValue >> 32;
                ruleLeft = ruleLeft << 32;

                lastHaflMatch = symAddrLastHalfValue == ruleLeft;

                return And(firstHaflMatch,lastHaflMatch);
            }

            return false;
            //return If(firstHaflMatch,v6Addr.GetLastHalfValue() == rule.targetAddress.lastHalfValue,false);
        }


		public Zen<bool> Srv6Encap(Zen<Packetv6> pkt,Srv6Rule rule,string neiborDevice)
		{
			NetworkBuilder.srh = new SrHeader();
			Stack hs = NetworkBuilder.headerStack;

			NetworkBuilder.srh = new SrHeader();
			int len = rule.encapAddress.Length;

			NetworkBuilder.srh.LastEntry = len;
			NetworkBuilder.srh.SegmentLeft = 1;
			NetworkBuilder.srh.SegmentList = rule.encapAddress;

			hs.Push(pkt.GetIpHeader());

			Zen<Ipv6> src = pkt.GetIpHeader().GetSrcIp();
			Zen<Ipv6> dst = Ipv6.Parse(NetworkBuilder.srh.SegmentList[0]);
			Zen<Ipv6Header> outerHdr = Ipv6Header.Create(dst,src,0);

			return rForwardv6(outerHdr,this.v6FwTables.Rules,neiborDevice,0);
		}

		/*
		public Zen<bool> Srv6Encaps(Zen<Packetv6> pkt,Srv6Rule rule,string neiborDevice)
		{
			//NetworkBuilder.innerHdr = pkt.GetIpHeader();

			return Srv6Encap(pkt,rule,neiborDevice);
		}
		*/

			//simplified function
		public Zen<bool> Srv6HdrLookup(Zen<Packetv6> pkt,int idx,string neiborDevice)
		{
			if(idx >= this.Srv6Rule.Count){
				return rForwardv6(pkt.GetIpHeader(),this.v6FwTables.Rules,neiborDevice,0);
			}

			return If(SegmentMatches(pkt,this.Srv6Rule[idx]),Srv6Encap(pkt,this.Srv6Rule[idx],neiborDevice),Srv6HdrLookup(pkt,idx+1,neiborDevice));
		}

		//simplified function
		public Zen<bool> Srv6ServiceChain(Zen<Packetv6> pkt, string neiborDevice)
		{			
			int idx = 0;
			Stack hs = NetworkBuilder.headerStack;
			SrHeader srh = NetworkBuilder.srh;
			
			if(hs.Count != 0){
				srh.SegmentLeft -= 1;
				if(srh.SegmentLeft == 0){
					srh = null;
					Zen<Ipv6Header> hdr = (Zen<Ipv6Header>)NetworkBuilder.headerStack.Peek();
					return rForwardv6(hdr,this.v6FwTables.Rules,neiborDevice,0);
				}
				else{
					Zen<Ipv6> src = pkt.GetIpHeader().GetSrcIp();
					Zen<Ipv6> dst = Ipv6.Parse((string)NetworkBuilder.headerStack.Pop());
					Zen<Ipv6Header> outerHdr = Ipv6Header.Create(dst,src,0);

					return rForwardv6(outerHdr,this.v6FwTables.Rules,neiborDevice,0);
				}
			}
			else{
				return Srv6HdrLookup(pkt,idx,neiborDevice);
			}
			
		}

        public Zen<bool> Run(Zen<Packetv6> pkt,string neiDevice)
        {
            Zen<bool> truth = true;

            if(reachabilityFunction != null){
                Zen<bool> tmpBool;
                foreach (ReachabilityFunction func in reachabilityFunction.GetInvocationList()){
                    tmpBool = func(pkt,neiDevice);
                    truth = And(tmpBool,truth);
                }
            }
            else{
                truth = false;
            }

            return truth;
        }
    }
}
