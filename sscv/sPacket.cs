namespace batzen
{
    using System.Diagnostics.CodeAnalysis;
    using ZenLib;
    using static ZenLib.Language;


    public struct sPacket
    {
        public EthHeader EthHeader;

        public Ipv4Header IpHeader;

        public UdpHeader UdpHeader;

        public static Zen<sPacket> Create(
            Zen<EthHeader> ethHeader,
            Zen<Ipv4Header> ipHeader,
            Zen<UdpHeader> udpHeader)
        {
            return Language.Create<sPacket>(
                ("EthHeader", ethHeader),
                ("IpHeader", ipHeader),
                ("UdpHeader",udpHeader));
        }
    }

    static class sPacketExtensions
    {
        public static Zen<EthHeader> GetEthHeader(this Zen<sPacket> packet)
        {
            return packet.GetField<sPacket, EthHeader>("EthHeader");
        }

        public static Zen<Ipv4Header> GetIpHeader(this Zen<sPacket> packet)
        {
            return packet.GetField<sPacket, Ipv4Header>("IpHeader");
        }

        public static Zen<UdpHeader> GetUdpHeader(this Zen<sPacket> packet)
        {
            return packet.GetField<sPacket, UdpHeader>("UdpHeader");
        }
    }
}