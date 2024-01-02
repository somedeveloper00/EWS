using System;
using System.Threading;
using System.Threading.Tasks;

public static class EwsUtilities
{
    /// <summary>
    /// the byte sequence that is used for confirming that the connection is an EWS connection.
    /// </summary>
    public static byte[] EwsSecret = new byte[] { 0x23, 0x33, 0x23, 0x33, 0x23, 0x33, 0x23, 0x33 };

    /// <summary>
    /// byte sequence sent from server to client, indicating the secret was accepted.
    /// </summary>
    public static byte[] SecretAccepted = new byte[] { 0x0, 0x0, 0x0, 0x0 };

    /// <summary>
    /// Compares two arrays.
    /// </summary>
    /// <param name="startIndex">(inclusive) The index to start comparing from.</param>
    /// <param name="endIndex">(inclusive) The index to stop comparing at.</param>
    public static bool SequenceEqual<T>(this T[] self, T[] other)
    {
        if (self.Length != other.Length)
        {
            return false;
        }

        for (int i = 0; i < self.Length; i++)
        {
            if (!self[i].Equals(other[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static Task WithToken(this Task task, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<bool>();
        ct.Register(() =>
        {
            tcs.TrySetCanceled();
        });
        task.ContinueWith(t =>
        {
            if (t.IsCanceled)
            {
                tcs.TrySetCanceled();
            }
            else if (t.IsFaulted)
            {
                tcs.TrySetException(t.Exception);
            }
            else
            {
                tcs.TrySetResult(true);
            }
        });
        return tcs.Task;
    }
}