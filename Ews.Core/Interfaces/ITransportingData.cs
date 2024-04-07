namespace Ews.Core.Interfaces
{
    /// <summary>
    /// Represents a data that can be sent and received through EWS
    /// </summary>
    public interface ITransportingData
    {
        public byte EventId { get; }
    }
}