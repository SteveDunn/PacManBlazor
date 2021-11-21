using System;
using System.IO;
using System.Text;

namespace PacMan;

public class ExceptionNotificationService : TextWriter, IExceptionNotificationService
{
    private readonly TextWriter _decorated;

    public override Encoding Encoding => Encoding.UTF8;

    /// <summary>
    /// Raised is an exception occurs. The exception message will be send to the listeners
    /// </summary>
    public event EventHandler<string> OnException;

    public ExceptionNotificationService()
    {
        _decorated = Console.Error;

        Console.SetError(this);
    }

    public override void WriteLine(string value)
    {
        // notify the listeners
        OnException?.Invoke(this, value);

        _decorated.WriteLine(value);
    }
}