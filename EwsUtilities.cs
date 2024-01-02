public static class EwsUtilities
{
    /// <summary>
    /// the byte sequence that is used for confirming that the connection is an EWS connection.
    /// </summary>
    public static byte[] EwsSecret = new byte[] { 0x23, 0x33, 0x23, 0x33, 0x23, 0x33, 0x23, 0x33 };

    /// <summary>
    /// byte sequence sent from server to client, indicating the secret was accepted.
    /// </summary>
    public static byte[] SecretAccepted = new byte[] { 1 };

    /// <summary>
    /// Compares two arrays.
    /// </summary>
    /// <param name="startIndex">(inclusive) The index to start comparing from.</param>
    /// <param name="endIndex">(inclusive) The index to stop comparing at.</param>
    public static bool SequenceEqual<T>(this T[] self, T[] other, int startIndex, int endIndex)
    {
        if (self.Length != other.Length)
        {
            return false;
        }

        for (int i = startIndex; i <= endIndex; i++)
        {
            if (!self[i].Equals(other[i]))
            {
                return false;
            }
        }

        return true;
    }
}