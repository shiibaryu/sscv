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
    public struct Packet
    {
        public Ipv4Header IpHeader { get; set; }

        public static Zen<Packet> Create(
            Zen<Ipv4Header> ipHeader)
        {
            return Language.Create<Packet>(
                ("IpHeader", ipHeader)
            );
        }

        /// <summary>
        /// Convert a packet to a string.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return $"packet({this.IpHeader})";
        }
    }

    /// <summary>
    /// Packet extension methods.
    /// </summary>
    [ExcludeFromCodeCoverage]
    static class PacketExtensions
    {
        public static Zen<Ipv4Header> GetIpHeader(this Zen<Packet> packet)
        {
            return packet.GetField<Packet, Ipv4Header>("IpHeader");
        }
    }
}