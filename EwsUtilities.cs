public static class EwsUtilities
{
    /// <summary>
    /// The length of the secret sent by the client to the server.
    /// </summary>
    public const int EwsSecretLength = 8;

    /// <summary>
    /// the byte sequence that is used for confirming that the connection is an EWS connection.
    /// </summary>
    public static byte[] EwsSecret = new byte[] { 0x23, 0x33, 0x23, 0x33, 0x23, 0x33, 0x23, 0x33 };
}