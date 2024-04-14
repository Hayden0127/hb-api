namespace HB.Service
{
    public interface IRunningSequenceService
    {
        Task<string> GetTransactionIdAsync();
    }
}