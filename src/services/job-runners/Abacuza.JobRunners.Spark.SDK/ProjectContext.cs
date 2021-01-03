using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK
{
    /// <summary>
    /// Represents the data that contains project and revision information.
    /// </summary>
    public sealed class ProjectContext
    {

        public ProjectContext(Guid projectId, string projectName, DateTime projectCreationDate, Guid revisionId)
            => (ProjectId, ProjectName, ProjectCreationDate, RevisionId) = (projectId, projectName, projectCreationDate, revisionId);

        /// <summary>
        /// Gets the id of the project.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        public string ProjectName { get; }

        /// <summary>
        /// Gets the creation date of the project.
        /// </summary>
        public DateTime ProjectCreationDate { get; }

        /// <summary>
        /// Gets the id of the revision.
        /// </summary>
        public Guid RevisionId { get; }
    }
}
