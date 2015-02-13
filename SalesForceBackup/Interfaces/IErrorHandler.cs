using System;

namespace SalesForceBackup.Interfaces
{
    /// <summary>
    /// Interface to handle application exceptions.
    /// </summary>
    public interface IErrorHandler
    {
        void HandleError(Exception e);
        void HandleError(Exception e, int exitCode);
        void HandleError(Exception e, int exitCode, string errorMessage);
    }
}
