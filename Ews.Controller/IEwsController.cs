using Ews.Core;

namespace Ews.Controller
{
    /// <summary>
    /// Base controller class. Do not use it directly!
    /// </summary>
    public interface IEwsController { }

    /// <summary>
    /// Represents an event listener for EWS client.
    /// </summary>
    public interface IEwsController<in T> : IEwsController
    {
        /// <summary>
        /// Event ID to listen to.
        /// </summary>
        byte EventId { get; }

        /// <summary>
        /// Process the data received from the EWS client.
        /// </summary>
        void Process(T data, EwsClient client);
    }
}