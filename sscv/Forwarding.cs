namespace batzen
{
    using System;
    using ZenLib;
    using static ZenLib.Language;

    public class Forwarding
    {
        //src,dst,pkt,ttl

        public Zen<bool> dForward(Zen<Packet> pkt,Device device,Device neiDevice)
        {
            return Forward(pkt,device.FwTables.Rules,neiDevice,0);
        }

        public Zen<bool> Forward(Zen<Packet> packet,ForwardingRule[] rules,Device neiborDevice,int i)
        {
            if (i == rules.Length)
            {
                return Constant<bool>(false);
            }

            var rule = rules[i];
    
            return If(
                And(rule.Matches(packet.GetIpHeader()),rule.Interface.Neighbor.Owner == neiborDevice,rule.Interface.State == "UP"),
                true,
                Forward(packet,rules,neiborDevice,i + 1)
            );
        }

        
        public Zen<bool> rForward(Zen<Packetv6> pkt,Device curDevice,Device dstDevice,int idx,int ttl)
        {
            ForwardingRulev6[] rules = curDevice.v6FwTables.Rules;

            if(rules.Length == idx){
                return false;
            }
            
            return If(rules[idx].Matches(pkt.GetIpHeader()),allForwardingPathCheck(pkt,rules[idx].Interface.Neighbor.Owner,dstDevice,ttl),rForward(pkt,curDevice,dstDevice,idx+1,ttl));
        }

        public Zen<bool> allForwardingPathCheck(Zen<Packetv6> pkt, Device curDevice,Device dstDevice,int ttl)
        {
            if(curDevice == dstDevice){
                return true;
            }

            if(ttl > 15){
                return false;
            }

            ttl += 1;

            return rForward(pkt,curDevice,dstDevice,0,ttl);    
        }
        
    }
}