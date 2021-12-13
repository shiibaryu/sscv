namespace batzen
{
    using ZenLib;
    using static ZenLib.Language;

    public class ReachabilityFunction
    {
        public delegate Zen<bool> PreRoutingDelgate(Zen<Packetv6> pkt,Device device,Device neiDevice);
        public PreRoutingDelgate preRouting;

        public delegate Zen<bool> ForwardDelegate(Zen<Packetv6> pkt,Device device,Device neiDevice);
        public ForwardDelegate forward;

        public delegate Zen<bool> PostRoutingDelegate(Zen<Packetv6> pkt,Device device,Device neiDevice);
        public PostRoutingDelegate postRouting;

        public Zen<bool> Run(Zen<Packetv6> pkt, Device device,Device neiDevice)
        {
            Zen<bool> preBool=true,forwardBool=true,postBool=true;

            if(preRouting != null){
                Zen<bool> tmpBool;
                foreach(PreRoutingDelgate pre in preRouting.GetInvocationList()){
                    tmpBool = preRouting(pkt,device,neiDevice);
                    preBool = And(tmpBool,preBool);
                }
            }

            if(forward != null){
                Zen<bool> tmpBool;
                foreach(ForwardDelegate forward in forward.GetInvocationList()){
                    tmpBool = forward(pkt,device,neiDevice);
                    forwardBool = And(tmpBool,forwardBool);
                }
            }

            if(postRouting != null){
                Zen<bool> tmpBool;
                 foreach(PostRoutingDelegate post in postRouting.GetInvocationList()){
                    tmpBool = postRouting(pkt,device,neiDevice);
                    postBool = And(tmpBool,postBool);
                }
            }

            return And(preBool,forwardBool,postBool);
        }
    }
}