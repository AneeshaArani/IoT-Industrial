// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Publisher.Stack.Transport
{
    using Azure.IIoT.OpcUa.Publisher.Stack.Transport.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;

    /// <summary>
    /// Network information extensions
    /// </summary>
    public static class NetworkInformationEx
    {
        /// <summary>
        /// Get all interface addresses
        /// </summary>
        /// <param name="netclass"></param>
        /// <returns></returns>
        public static IEnumerable<NetInterface> GetAllNetInterfaces(
            NetworkClass netclass)
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(n =>
                    n.NetworkInterfaceType.IsInClass(netclass) &&
                    n.OperationalStatus == OperationalStatus.Up &&
                    n.GetIPProperties()?.GatewayAddresses.Count > 0)
                .SelectMany(n => n.GetIPProperties().UnicastAddresses
                    .Select(x => new NetInterface(n.Name,
                        n.GetPhysicalAddress(),
                        x.Address,
                        x.IPv4Mask,
                        n.GetIPProperties().GatewayAddresses
                            .First(a => a != null).Address,
                        n.GetIPProperties().DnsSuffix,
                        n.GetIPProperties().DnsAddresses)))
                .Where(t =>
                    t.UnicastAddress.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(t.UnicastAddress))
                .Distinct();
        }

        /// <summary>
        /// Check whether the interface type fits the class
        /// </summary>
        /// <param name="type"></param>
        /// <param name="netclass"></param>
        /// <returns></returns>
        public static bool IsInClass(this NetworkInterfaceType type,
            NetworkClass netclass)
        {
            switch (type)
            {
                case NetworkInterfaceType.Ethernet:
                case NetworkInterfaceType.Ethernet3Megabit:
                case NetworkInterfaceType.GigabitEthernet:
                case NetworkInterfaceType.FastEthernetT:
                case NetworkInterfaceType.FastEthernetFx:
                case NetworkInterfaceType.Slip:
                case NetworkInterfaceType.IPOverAtm:
                    return (netclass & NetworkClass.Wired) != 0;

                case NetworkInterfaceType.BasicIsdn:
                case NetworkInterfaceType.PrimaryIsdn:
                case NetworkInterfaceType.Isdn:
                case NetworkInterfaceType.GenericModem:
                case NetworkInterfaceType.AsymmetricDsl:
                case NetworkInterfaceType.SymmetricDsl:
                case NetworkInterfaceType.RateAdaptDsl:
                case NetworkInterfaceType.VeryHighSpeedDsl:
                case NetworkInterfaceType.MultiRateSymmetricDsl:
                case NetworkInterfaceType.Ppp:
                    return (netclass & NetworkClass.Modem) != 0;

                case NetworkInterfaceType.Wireless80211:
                case NetworkInterfaceType.Wman:
                case NetworkInterfaceType.Wwanpp:
                case NetworkInterfaceType.Wwanpp2:
                    return (netclass & NetworkClass.Wireless) != 0;
                case NetworkInterfaceType.Tunnel:
                    return (netclass & NetworkClass.Tunnel) != 0;

                case NetworkInterfaceType.TokenRing:
                case NetworkInterfaceType.HighPerformanceSerialBus:
                case NetworkInterfaceType.Fddi:
                case NetworkInterfaceType.Atm:
                case NetworkInterfaceType.Loopback:
                    return false;
            }
            return false;
        }
    }
}
