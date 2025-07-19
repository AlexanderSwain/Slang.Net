namespace Slang.Sdk.Interop;

/// <summary>
/// Exception thrown when a Slang operation fails.
/// </summary>
public class SlangException : Exception
{
    /// <summary>
    /// The Slang result code that caused this exception.
    /// </summary>
    public SlangResult Result { get; }

    public SlangException(SlangResult result) : base(GetMessageForResult(result))
    {
        Result = result;
    }

    public SlangException(SlangResult result, string message) : base(message)
    {
        Result = result;
    }

    public SlangException(SlangResult result, string message, Exception innerException) : base(message, innerException)
    {
        Result = result;
    }

    private static string GetMessageForResult(SlangResult result)
    {
        return result switch
        {
            SlangResult.Ok => "Success",
            SlangResult.Fail => "Generic failure",
            SlangResult.NoInterface => "Interface not supported",
            SlangResult.Abort => "Operation aborted",
            SlangResult.InvalidArg => "Invalid argument",
            SlangResult.NotImplemented => "Not implemented",
            SlangResult.OutOfMemory => "Out of memory",
            SlangResult.Pointer => "Invalid pointer",
            SlangResult.Handle => "Invalid handle",
            SlangResult.CompilationFailed => "Compilation failed",
            SlangResult.InternalError => "Internal compiler error",
            _ => $"Unknown error (code: {(int)result})"
        };
    }
}

/// <summary>
/// Helper methods for working with Slang results.
/// </summary>
public static class SlangResultHelper
{
    /// <summary>
    /// Checks if a SlangResult indicates success.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <returns>True if the result indicates success, false otherwise.</returns>
    public static bool IsSuccess(SlangResult result)
    {
        return (int)result >= 0;
    }

    /// <summary>
    /// Checks if a SlangResult indicates failure.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <returns>True if the result indicates failure, false otherwise.</returns>
    public static bool IsFailure(SlangResult result)
    {
        return (int)result < 0;
    }

    /// <summary>
    /// Throws a SlangException if the result indicates failure.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="SlangException">Thrown if the result indicates failure.</exception>
    public static void ThrowOnFailure(SlangResult result, string? message = null)
    {
        if (IsFailure(result))
        {
            if (message != null)
                throw new SlangException(result, message);
            else
                throw new SlangException(result);
        }
    }

    /// <summary>
    /// Throws a SlangException if the result indicates failure, otherwise returns the result.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The result if it indicates success.</returns>
    /// <exception cref="SlangException">Thrown if the result indicates failure.</exception>
    public static SlangResult EnsureSuccess(SlangResult result, string? message = null)
    {
        ThrowOnFailure(result, message);
        return result;
    }
}