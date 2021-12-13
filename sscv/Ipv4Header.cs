// <copyright file="IpHeader.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace batzen
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using ZenLib;

    /// <summary>
    /// An IP header object.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Ipv4Header
    {
        /// <summary>
        /// The destination Ip.
        /// </summary>
        public Ipv4 DstIp { get; set; }

        /// <summary>
        /// The source Ip.
        /// </summary>
        public Ipv4 SrcIp { get; set; }


        /*
        /// <summary>
        /// The destination port.
        /// </summary>
        public ushort DstPort { get; set; }

        /// <summary>
        /// The source port.
        /// </summary>
        public ushort SrcPort { get; set; }
        */

        /// <summary>
        /// The IP protocol.
        /// </summary>
        public byte Protocol { get; set; }

    
        public static Zen<Ipv4Header> Create(
            Zen<Ipv4> dstIp,
            Zen<Ipv4> srcIp)
        {
            return Language.Create<Ipv4Header>(
                ("DstIp", dstIp),
                ("SrcIp", srcIp));
        }
        /*
        public static Zen<Ipv4Header> Create(
            Zen<Ipv4> dstIp,
            Zen<Ipv4> srcIp,
            Zen<ushort> dstPort,
            Zen<ushort> srcPort,
            Zen<byte> protocol)
        {
            return Language.Create<Ipv4Header>(
                ("DstIp", dstIp),
                ("SrcIp", srcIp),
                ("DstPort", dstPort),
                ("SrcPort", srcPort),
                ("Protocol", protocol));
        }
        */

        /// <summary>
        /// Equality for ip headers.
        /// </summary>
        /// <param name="obj">The other header.</param>
        /// <returns>True or false.</returns>
        /*
        public override bool Equals(object obj)
        {
            return obj is IpHeader header &&
                   EqualityComparer<Ip>.Default.Equals(DstIp, header.DstIp) &&
                   EqualityComparer<Ip>.Default.Equals(SrcIp, header.SrcIp) &&
                   DstPort == header.DstPort &&
                   SrcPort == header.SrcPort &&
                   Protocol == header.Protocol;
        }

        /// <summary>
        /// Hashcode for ip headers.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(DstIp, SrcIp, DstPort, SrcPort, Protocol);
        }
        */

        /// <summary>
        /// Convert a packet to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"header(src={SrcIp}, dst={DstIp}, proto={Protocol})";
        }
        
    }

    /// <summary>
    /// IP header extension methods.
    /// </summary>
    [ExcludeFromCodeCoverage]
    static class IpHeaderExtensions
    {
        public static Zen<Ipv4> GetDstIp(this Zen<Ipv4Header> hdr)
        {
            return hdr.GetField<Ipv4Header, Ipv4>("DstIp");
        }

        public static Zen<Ipv4> GetSrcIp(this Zen<Ipv4Header> hdr)
        {
            return hdr.GetField<Ipv4Header, Ipv4>("SrcIp");
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

        public static Zen<byte> GetProtocol(this Zen<Ipv4Header> hdr)
        {
            return hdr.GetField<Ipv4Header, byte>("Protocol");
        }

    }
}
