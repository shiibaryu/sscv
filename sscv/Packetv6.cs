// <copyright file="Packet.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace batzen
{
    using System.Diagnostics.CodeAnalysis;
    using ZenLib;
    using static ZenLib.Language;

    /// <summary>
    /// Packet struct that contains IP headers.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public struct Packetv6
    {
        public Ipv6Header IpHeader { get; set; }

        public static Zen<Packetv6> Create(
            Zen<Ipv6Header> ipHeader)
        {
            return Language.Create<Packetv6>(
                ("IpHeader", ipHeader)
            );
        }

        public override string ToString()
        {
            //return $"packet({this.IpHeader})";
			return $"packet({IpHeader})";
		}
    }

	
    static class Packetv6Extensions
    {
        public static Zen<Ipv6Header> GetIpHeader(this Zen<Packetv6> packet)
        {
            return packet.GetField<Packetv6, Ipv6Header>("IpHeader");
        }
    }
}