namespace Ews.Core.Interfaces
{
    /// <summary>
    /// Represents a listener for the EWS protocol.
    /// </summary>
    public interface IEwsEventListener
    {
        /// <summary>
        /// Processes the given bytes.
        /// </summary>
        void Process(EwsClient client, byte[] bytes);
    }
}
