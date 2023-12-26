namespace EWS.Interfaces
{
    /// <summary>
    /// Represents an encryption algorithm used for sending and receiving data.
    /// </summary>
    public interface IEncryption
    {
        /// <summary>
        /// Encrypts the given bytes.
        /// </summary>
        /// <param name="client">the client that is sending the bytes.</param>
        /// <param name="bytes">the bytes to encrypt. (changing its size is acceptable)</param>
        /// <param name="error">error message if the encryption failed.</param>
        void Encrypt(EwsClient client, ref byte[] bytes, out string error);

        /// <summary>
        /// Decrypts the given bytes.
        /// </summary>
        /// <param name="client">the client that is receiving the bytes.</param>
        /// <param name="bytes">the bytes to decrypt. (changing its size is acceptable)</param>
        /// <param name="error">error message if the decryption failed.</param>
        string Decrypt(EwsClient client, ref byte[] bytes, out string error);
    }
}