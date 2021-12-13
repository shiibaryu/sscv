namespace batzen
{
    using System.Collections.Generic;

    public class Srv6Rule
    {
        //address index when encap
        public int index;

        public int prefix;

        public Ipv6 targetAddress;

        public string mode;

        public string[] encapAddress;
    }   
}