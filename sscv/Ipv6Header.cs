namespace batzen
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using ZenLib;

    public class Ipv6Header
    {
        public Ipv6 DstIp { get; set; }

        public Ipv6 SrcIp { get; set; }

        public int Length { get; set; }

        public static Zen<Ipv6Header> Create(
            Zen<Ipv6> dstIp,
            Zen<Ipv6> srcIp,
            Zen<int> Length)
        {
            return Language.Create<Ipv6Header>(
                ("DstIp", dstIp),
                ("SrcIp", srcIp),
                ("Length", Length));
        }

        
        public override string ToString()
        {
            return $"header(src={SrcIp}, dst={DstIp}, len={Length})";
        }
    }

    
    static class Ipv6HeaderExtensions
    {
        public static Zen<Ipv6> GetDstIp(this Zen<Ipv6Header> hdr)
        {
            return hdr.GetField<Ipv6Header, Ipv6>("DstIp");
        }

        public static Zen<Ipv6> GetSrcIp(this Zen<Ipv6Header> hdr)
        {
            return hdr.GetField<Ipv6Header, Ipv6>("SrcIp");
        }

        /*        
        public static Zen<ushort> GetDstPort(this Zen<IpHeader> hdr)
        {
            return hdr.GetField<IpHeader, ushort>("DstPort");
        }

        public static Zen<ushort> GetSrcPort(this Zen<IpHeader> hdr)
        {
            return hdr.GetField<IpHeader, ushort>("SrcPort");
        }
        */

        public static Zen<int> GetLength(this Zen<Ipv6Header> hdr)
        {
            return hdr.GetField<Ipv6Header, int>("Length");
        }

    }
}
