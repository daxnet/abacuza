using System.Diagnostics.Contracts;
namespace Abacuza.JobSchedulers.Common.Models
{
    /// <summary>
    /// Represents the job states.
    /// </summary>
    public enum JobState
    {
        Created,
        Queued,
        Started,
        Completed,
        Cancelled,
        Failed
    }
}