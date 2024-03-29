namespace Abacuza.Jobs.ApiService.Models
{
    /// <summary>
    /// Represents the job states.
    /// </summary>
    public enum JobState
    {
        Unknown,
        Created,
        Initializing,
        Running,
        Busy,
        Completed,
        Cancelled,
        Failed
    }
}