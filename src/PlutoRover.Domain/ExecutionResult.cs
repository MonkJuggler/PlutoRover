namespace PlutoRover.Domain
{
    public class ExecutionResult
    {
        public ExecutionResult(Status status, string message)
        {
            Status = status;
            Message = message;
        }

        public Status Status { get; private set; }
        public string Message { get; private set; }
    }
}