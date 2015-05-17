using System;

namespace Serilog.Context
{
    /// <summary>
    /// Determines what log entries should be written for an operation.
    /// </summary>
    [Flags]
    public enum OperationContextLogMode
    {
        /// <summary>
        /// Write no log entries for the operation.
        /// </summary>
        None = 0,

        /// <summary>
        /// Write a log entry for the start of the operation.
        /// </summary>
        Start = 1,

        /// <summary>
        /// Write a log entry for the end of the operation.
        /// </summary>
        End = 2,

        /// <summary>
        /// Write a log entry for both the start and end of the operation.
        /// </summary>
        StartAndEnd = 3,

        /// <summary>
        /// Use in combination with writing the end operation. The end operation will only be written if a warning or worse occured.
        /// </summary>
        /// <remarks>This has no effect on the start operation being written.</remarks>
        WarningOrWorse = 4,

        /// <summary>
        /// Write a log entry for the end of the operation but only if a warning or worse occurred.
        /// </summary>
        EndOnlyOnWarningOrWorse = 6,

        /// <summary>
        /// Write a log entry for both the start and end of the operation, but only write the end entry if a warning or worse occurred.
        /// </summary>
        StartAndEndOnlyOnWarningOrWorse = 7,

        /// <summary>
        /// Use in combination with writing the end operation. The end operation will only be written if an error occured.
        /// </summary>
        /// <remarks>This has no effect on the start operation being written.</remarks>
        Error = 8,

        /// <summary>
        /// Write a log entry for the end of the operation but only if an error occurred.
        /// </summary>
        EndOnlyOnError = 10,

        /// <summary>
        /// Write a log entry for both the start and end of the operation, but only write the end entry if an error occurred.
        /// </summary>
        StartAndEndOnlyOnError = 11
    }
}