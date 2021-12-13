namespace batzen
{
    using ZenLib;

    using System.Collections.Generic;
    
    public class OverlayPacket
    {
        public int idx = 0;

        public List<string> encapList = new List<string>();

        public Zen<Packetv6> currentPkt = null;
    }
}