using System.Net;

/// <summary>
/// Specifies the IP address of a network interface (network card) in a machine.
/// </summary>
public enum IPAddressEnum
{
    /// <summary>
    /// Provides an IP address that indicates that the server must listen for client activity on all network interfaces. This field is read-only.
    /// </summary>
    Any,

    /// <summary>
    /// Provides the IP broadcast address. This field is read-only.
    /// </summary>
    Broadcast,

    /// <summary>
    /// The System.Net.Sockets.Socket.Bind(System.Net.EndPoint) method uses the System.Net.IPAddress.IPv6Any field to indicate that a System.Net.Sockets.Socket must listen for client activity on all network interfaces.
    /// </summary>
    IPv6Any,

    /// <summary>
    /// Provides the IP loopback address. This property is read-only.
    /// </summary>
    IPv6Loopback,

    /// <summary>
    /// Provides an IP address that indicates that no network interface should be used. This property is read-only.
    /// </summary>
    IPv6None,

    /// <summary>
    /// Provides the IP loopback address. This field is read-only.
    /// </summary>
    Loopback,

    /// <summary>
    /// Provides an IP address that indicates that no network interface should be used. This field is read-only.
    /// </summary>
    None,
}

public static class IPAddressEnumExtensions
{
    /// <summary>
    /// Converts the <see cref="IPAddressEnum"/> to an <see cref="IPAddress"/>.
    /// </summary>
    public static IPAddress ToIpAddress(this IPAddressEnum iPAddress)
    {
        return iPAddress switch
        {
            IPAddressEnum.Any => IPAddress.Any,
            IPAddressEnum.Broadcast => IPAddress.Broadcast,
            IPAddressEnum.IPv6Any => IPAddress.IPv6Any,
            IPAddressEnum.IPv6Loopback => IPAddress.IPv6Loopback,
            IPAddressEnum.IPv6None => IPAddress.IPv6None,
            IPAddressEnum.Loopback => IPAddress.Loopback,
            _ => IPAddress.None,
        };
    }
}