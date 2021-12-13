namespace batzen
{
    using System;
    using System.Net;
    using ZenLib;
    public struct Ipv6
    {
        public ulong firstHalfValue;
        
        public ulong lastHalfValue;

        public static Zen<Ipv6> Create(Zen<ulong> firstHalf,Zen<ulong> lastHalf)
        {
            return Language.Create<Ipv6>(("firstHalfValue", firstHalf),("lastHalfValue",lastHalf));
        }

        public static Ipv6 Parse(string addr)
        {
            IPNetwork ipNetwork = IPNetwork.Parse(addr);
            byte[] bytes = ipNetwork.Network.GetAddressBytes();
        
            return new Ipv6{firstHalfValue = BitConverter.ToUInt64(bytes),lastHalfValue = BitConverter.ToUInt64(bytes,8)};
        }   
        
        public override string ToString()
        {   
            var x1 = (this.firstHalfValue >> 0) & 0x00000000000000FF;
            var x2 = (this.firstHalfValue >> 8) & 0x00000000000000FF;
            var x3 = (this.firstHalfValue >> 16) & 0x00000000000000FF;
            var x4 = (this.firstHalfValue >> 24) & 0x00000000000000FF;
            var x5 = (this.firstHalfValue >> 32) &  0x00000000000000FF;
            var x6 = (this.firstHalfValue >> 40) & 0x00000000000000FF;
            var x7 = (this.firstHalfValue >> 48) & 0x00000000000000FF;
            var x8 = (this.firstHalfValue >> 56) & 0x00000000000000FF;

            var x9 = (this.lastHalfValue >> 0) & 0x00000000000000FF;
            var x10 = (this.lastHalfValue >> 8) & 0x00000000000000FF;
            var x11 = (this.lastHalfValue >> 16) & 0x00000000000000FF;
            var x12 = (this.lastHalfValue >> 24) & 0x00000000000000FF;
            var x13 = (this.lastHalfValue >> 32) &  0x00000000000000FF;
            var x14 = (this.lastHalfValue >> 40) & 0x00000000000000FF;
            var x15 = (this.lastHalfValue >> 48) & 0x00000000000000FF;
            var x16 = (this.lastHalfValue >> 56) & 0x00000000000000FF;

            return $"{string.Format("{0:x2}",x1)}{string.Format("{0:x2}",x2)}:{string.Format("{0:x2}",x3)}{string.Format("{0:x2}",x4)}:{string.Format("{0:x2}",x5)}{string.Format("{0:x2}",x6)}:{string.Format("{0:x2}",x7)}{string.Format("{0:x2}",x8)}:{string.Format("{0:x2}",x9)}{string.Format("{0:x2}",x10)}:{string.Format("{0:x2}",x11)}{string.Format("{0:x2}",x12)}:{string.Format("{0:x2}",x13)}{string.Format("{0:x2}",x14)}:{string.Format("{0:x2}",x15)}{string.Format("{0:x2}",x16)}";
        }

        public static byte[] getIpv6Low(string addr)
        {
            IPNetwork ipNetwork = IPNetwork.Parse(addr);

            return ipNetwork.FirstUsable.GetAddressBytes();
        }

        public static byte[] getIpv6High(string addr)
        {
            IPNetwork ipNetwork = IPNetwork.Parse(addr);
            return ipNetwork.LastUsable.GetAddressBytes();
        }
    }

    static class Ipv6Extensions
    {
        public static Zen<ulong> GetFirstHalfValue(this Zen<Ipv6> ip)
        {
            return ip.GetField<Ipv6, ulong>("firstHalfValue");
        }

        public static Zen<ulong> GetLastHalfValue(this Zen<Ipv6> ip)
        {
            return ip.GetField<Ipv6, ulong>("lastHalfValue");
        }
    }
}