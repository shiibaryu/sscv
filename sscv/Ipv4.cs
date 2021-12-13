// <copyright file="Ip.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace batzen
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using ZenLib;

    /// <summary>
    /// A simple IP address.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public struct Ipv4
    {
        /// <summary>
        /// The value for the ip.
        /// </summary>
        public uint Value { get; set; }

        /// <summary>
        /// Create a Zen ip from a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The ip.</returns>
        public static Zen<Ipv4> Create(Zen<uint> value)
        {
            return Language.Create<Ipv4>(("Value", value));
        }

        /// <summary>
        /// Parse an ip from a string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>The ip.</returns>
        public static Ipv4 Parse(string s)
        {
            var bytes = IPAddress.Parse(s).GetAddressBytes();
            return FromBytes(bytes[0], bytes[1], bytes[2], bytes[3]);
        }

        /// <summary>
        /// Convert the ip to a string format.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            var x1 = (this.Value >> 0) & 0x000000FF;
            var x2 = (this.Value >> 8) & 0x000000FF;
            var x3 = (this.Value >> 16) & 0x000000FF;
            var x4 = (this.Value >> 24) & 0x000000FF;
            return $"{x4}.{x3}.{x2}.{x1}";
        }

        /// <summary>
        /// Create an ip from bytes.
        /// </summary>
        /// <param name="x1">The first octet.</param>
        /// <param name="x2">The second octet.</param>
        /// <param name="x3">The third octet.</param>
        /// <param name="x4">The fourth octet.</param>
        /// <returns>An ip address.</returns>
        public static Ipv4 FromBytes(byte x1, byte x2, byte x3, byte x4)
        {
            return new Ipv4 { Value = (uint)(x1 << 24) | (uint)(x2 << 16) | (uint)(x3 << 8) | x4 };
        }

        public static uint v4ToInt(string addr)
        {
            uint ip = 0;
            string[] elms = addr.Split(".");
            if(elms.Length == 4){
                ip = Convert.ToUInt32(elms[0]) << 24;
                ip += Convert.ToUInt32(elms[1]) << 16;
                ip += Convert.ToUInt32(elms[2]) << 8;
                ip += Convert.ToUInt32(elms[3]);
            }
            return ip;
        }

        public static string getIpv4Low(string addr)
        {
            string[] elms = addr.Split('/');
            
            uint ip = v4ToInt(elms[0]);
            uint mask = ~(0xffffffff >> Convert.ToInt32(elms[1]));

            return Convert.ToString((ip & mask)+1);
        }

        public static string getIpv4High(string addr)
        {
            string[] elms = addr.Split('/');
            
            uint ip = v4ToInt(elms[0]);
            uint mask = ~(0xffffffff >> Convert.ToInt32(elms[1]));

            return Convert.ToString(ip + ~mask - 1);
        }
    }

    /// <summary>
    /// Ip extension methods.
    /// </summary>
    [ExcludeFromCodeCoverage]
    static class IpExtensions
    {
        public static Zen<uint> GetValue(this Zen<Ipv4> ip)
        {
            return ip.GetField<Ipv4, uint>("Value");
        }
    }
}