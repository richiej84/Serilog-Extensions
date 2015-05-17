namespace Serilog.Context
{
    public delegate void OperationLogWriter(ILogger logger, OperationContextData data);
    //public delegate void TimedOperationLogWriter(ILogger logger, TimedOperationContext context);
}
