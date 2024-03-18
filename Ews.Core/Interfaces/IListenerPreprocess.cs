namespace Ews.Core.Interfaces
{
    /// <summary>
    /// Represents a preprocessor for calling <see cref="IEwsEventListener"/>s when receiving events from 
    /// <see cref="EwsClient"/>.
    /// </summary>
    public interface IListenerPreprocess
    {
        /// <summary>
        /// handles the execution of a listener apon receiving an event. 
        /// </summary>
        void ExecuteNewEvent(EwsClient client, byte[] message, IEwsEventListener listener);
    }
}