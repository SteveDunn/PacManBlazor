using System;

namespace PacMan
{
    public interface IExceptionNotificationService
    {
        /// <summary>
        /// Raised is an exception occurs. The exception message will be send to the listeners
        /// </summary>
        event EventHandler<string> OnException;
    }
}