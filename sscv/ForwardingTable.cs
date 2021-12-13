// <copyright file="ForwardingTable.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace batzen
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using ZenLib;
    using static ZenLib.Language;

    /// <summary>
    /// A forwarding table object.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ForwardingTable
    {
        public ForwardingRule[] Rules { get; set; }

        public void setConfiguration(int idx,RouteProperty routeProperty, Dictionary<string,Interface> Interface)
        {
            this.Rules[idx] = new ForwardingRule();

            string ip = routeProperty.IpAddress;
            string interfaceName = routeProperty.Interface;

            this.Rules[idx].DstIpHigh = Ipv4.Parse(Ipv4.getIpv4High(ip));
            this.Rules[idx].DstIpLow = Ipv4.Parse(Ipv4.getIpv4Low(ip));
            this.Rules[idx].Interface = Interface[interfaceName];
        }
    }

    /// <summary>
    /// A forwarding rule object.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ForwardingRule
    {
        public Ipv4 DstIpLow { get; set; }

        public Ipv4 DstIpHigh { get; set; }

        public Interface Interface { get; set; }

        public Zen<bool> Matches(Zen<Ipv4Header> hdr)
        {
            Zen<bool> dstLowMatch = hdr.GetDstIp().GetValue() >= this.DstIpLow.Value;
            Zen<bool> dstHighMatch = hdr.GetDstIp().GetValue() <= this.DstIpHigh.Value;
            return And(dstLowMatch, dstHighMatch);
        }
    }
}