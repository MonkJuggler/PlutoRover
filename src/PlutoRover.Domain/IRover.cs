namespace PlutoRover.Domain
{
    public interface IRover
    {
        ExecutionResult ExecuteCommands(string cmd);
    }
}
