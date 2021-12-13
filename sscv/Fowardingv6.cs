namespace batzen
{
    using System;
    using ZenLib;
    using static ZenLib.Language;

    public class Forwardingv6
    {
        public Zen<bool> dForward(Zen<Packetv6> pkt,Device device,Device neiborDevice)
        {
            return Forward(pkt,device.v6FwTables.Rules,neiborDevice,0);
        }

        public Zen<bool> Forward(Zen<Packetv6> packet,ForwardingRulev6[] rules,Device neiborDevice,int i)
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
        /*
        public Zen<bool> rForward(Zen<Packetv6> pkt,Device curDevice,Device dstDevice)
        {
            if(curDevice == dstDevice){
                return true;
            }

            ForwardingRulev6[] rules = curDevice.v6FwTables.Rules;
            int num = rules.Length;
            int i;
            Zen<bool> b = null;

            for(i=0;i<num;i++){
                b = And(rules[i].Matches(pkt.GetIpHeader()),rules[i].Interface.State == "UP");
                if(b.ToString() == "True"){
                    break;
                }
            }
            
            if(b.ToString() == "True"){
                return rForward(pkt,rules[i].Interface.Owner,dstDevice);    
            }

            return false;
        }
        */
    }
}