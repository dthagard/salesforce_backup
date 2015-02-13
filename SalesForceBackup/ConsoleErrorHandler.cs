using System;
using SalesForceBackup.Interfaces;

namespace SalesForceBackup
{
    /// <summary>
    /// Outputs all errors to the console.
    /// </summary>
    public class ConsoleErrorHandler : IErrorHandler
    {
        /// <summary>
        /// Handles error logging for the console application.
        /// </summary>
        /// <param name="e">The exception to handle.</param>
        /// <remarks>This will terminate the application with the specified exit code.</remarks>
        public void HandleError(Exception e)
        {
            HandleError(e, (int) Enums.ExitCode.Unknown);
        }

        /// <summary>
        /// Handles error logging for the console application.
        /// </summary>
        /// <param name="e">The exception to handle.</param>
        /// <param name="exitCode">Optionally, the error code to return to the console. Defaults to -1 (unknown).</param>
        /// <remarks>This will terminate the application with the specified exit code.</remarks>
        public void HandleError(Exception e, int exitCode)
        {
            HandleError(e, exitCode, "Unknown error occured:");
        }

        /// <summary>
        /// Handles error logging for the console application.
        /// </summary>
        /// <param name="e">The exception to handle.</param>
        /// <param name="exitCode">Optionally, the error code to return to the console. Defaults to -1 (unknown).</param>
        /// <param name="errorMessage">Optionally, a descriptive error message to return to the console.</param>
        /// <remarks>This will terminate the application with the specified exit code.</remarks>
        public void HandleError(Exception e, int exitCode, string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Console.WriteLine(e.ToString());
            Environment.Exit(exitCode);
        }
    }
}
