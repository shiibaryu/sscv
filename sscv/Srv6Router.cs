namespace batzen
{
    using ZenLib;
    using static ZenLib.Language;
    using System.Collections.Generic;
    public class Srv6Router
    {
        public Device DeviceInfo;

        public List<Srv6Rule> Srv6Rule {get;set;}

        public void setConfiguration(Device devInfo,List<Srv6Rule> rule)
        {
            this.DeviceInfo = devInfo;
            this.Srv6Rule = rule;
        }

        public Zen<bool> Matches(Zen<Packetv6> packet, Srv6Rule rule)
        {
            Zen<Ipv6> v6Addr = packet.GetIpHeader().GetDstIp();
            Zen<bool> firstHaflMatch = v6Addr.GetFirstHalfValue() == rule.targetAddress.firstHalfValue;
            return If(firstHaflMatch,v6Addr.GetLastHalfValue() == rule.targetAddress.lastHalfValue,false);
        }   
        public void transit(Zen<Packetv6> pkt)
        {
            foreach(var rule in this.Srv6Rule){
                Matches(pkt,rule);    
            }
        }   

    }
}