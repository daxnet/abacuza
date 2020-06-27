namespace Abacuza.JobSchedulers.Common.Models
{
    public sealed class JobResponse
    {
        /// <summary>
        /// Gets or sets the id of the job, it can also be a session id
        /// for the running job.
        /// </summary>
        /// <value></value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the lines of logs.
        /// </summary>
        /// <value></value>
        public string[] Logs { get; set; }

        /// <summary>
        /// Gets or sets the job status information.
        /// </summary>
        /// <value></value>
        public string State { get; set; }
    }
}