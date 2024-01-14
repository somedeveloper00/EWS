using System;

namespace EWS
{
    /// <summary>
    /// Represents an empty delegate.
    /// </summary>
    public delegate void EmptyDelegate();

    /// <summary>
    /// Represents a delegate that logs an error.
    /// </summary>
    /// <param name="msg">formatted string message</param>
    /// <param name="args">arguments for the message</param>
    public delegate void LogErrorDelegate(string msg, params object[] args);

    public static class EwsCommonDelegateExtensions
    {
        /// <summary>
        /// Invokes the <paramref name="logError"/> delegate formatted properly for an exception message.
        /// </summary>
        public static void Exception(this LogErrorDelegate logError, Exception ex)
        {
            logError?.Invoke("{0}:{1}\n{2}", ex.GetType().Name, ex.Message, ex.StackTrace);
        }
    }
}