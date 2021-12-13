namespace batzen
{
    using System.Collections.Generic;
    using System;
    using ZenLib;
    using static ZenLib.Language;

    public class ForwardingTablev6
    {
        public ForwardingRulev6[] Rules { get; set; }

        public void setConfiguration(int idx,RouteProperty routeProperty, Dictionary<string,Interface> Interface)
        {
            this.Rules[idx] = new ForwardingRulev6();

            string ip = routeProperty.IpAddress;
            string interfaceName = routeProperty.Interface;
            
			if(ip == "0.0.0.0/0"){
				this.Rules[idx].DstIpHigh = new Ipv6{firstHalfValue = 0,lastHalfValue = 0};
				this.Rules[idx].DstIpLow = new Ipv6{firstHalfValue = 0,lastHalfValue = 0};
            	this.Rules[idx].Interface = Interface[interfaceName];
			}
			else{
				byte[] addrHigh = Ipv6.getIpv6High(ip);
            	this.Rules[idx].DstIpHigh = new Ipv6{firstHalfValue = BitConverter.ToUInt64(addrHigh),lastHalfValue = BitConverter.ToUInt64(addrHigh,8)};
            	byte[] addrLow = Ipv6.getIpv6Low(ip);
            	this.Rules[idx].DstIpLow = new Ipv6{firstHalfValue = BitConverter.ToUInt64(addrLow),lastHalfValue = BitConverter.ToUInt64(addrLow,8)};
            	this.Rules[idx].Interface = Interface[interfaceName];
			}
           
        }
    }

    public class ForwardingRulev6
    {
        public Ipv6 DstIpLow { get; set; }

        public Ipv6 DstIpHigh { get; set; }

        public Interface Interface { get; set; }

        public Zen<bool> LowMatches(Zen<ulong> addr1,ulong addr2)
        {
            return addr1 > addr2;
        }

         public Zen<bool> HighMatches(Zen<ulong> addr1,ulong addr2)
        {
            return addr1 < addr2;
        }

         public Zen<bool> SecondLowMatches(Zen<Ipv6Header> hdr)
        {
            return If(hdr.GetDstIp().GetFirstHalfValue() == this.DstIpLow.firstHalfValue,LowMatches(hdr.GetDstIp().GetLastHalfValue(),this.DstIpLow.lastHalfValue),false);
        }

        public Zen<bool> SecondHighMatches(Zen<Ipv6Header> hdr)
        {
            return If(hdr.GetDstIp().GetFirstHalfValue() == this.DstIpHigh.firstHalfValue,HighMatches(hdr.GetDstIp().GetLastHalfValue(),this.DstIpHigh.lastHalfValue),false);
        }

        public Zen<bool> Matches(Zen<Ipv6Header> hdr)
        {   
            Zen<bool> dstFirstHalfHighMatch = hdr.GetDstIp().GetFirstHalfValue() > this.DstIpLow.firstHalfValue;
            Zen<bool> dstFirstHalfLowMatch = If(dstFirstHalfHighMatch,true,SecondLowMatches(hdr));
            Zen<bool> dstLastHalfHighMatch = hdr.GetDstIp().GetFirstHalfValue() <= this.DstIpHigh.firstHalfValue;
            Zen<bool> dstLastHalfLowMatch = If(dstLastHalfHighMatch,true,SecondHighMatches(hdr));
            /*
            if(this.DstIpHigh.lastHalfValue == 0xffffffffffffffff){
                Console.WriteLine("this.DstIpHigh.lastHalfValue == 0xffffffffffffffff");
                dstLastHalfLowMatch = true;
            }
            */
            //return And(dstFirstHalfLowMatch, dstLastHalfLowMatch);
            return And(dstFirstHalfLowMatch, dstLastHalfHighMatch);
        }
    }
}