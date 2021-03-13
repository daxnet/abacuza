// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020-2021 by daxnet. All rights reserved.
// Apache License Version 2.0
// ==============================================================

using System;

namespace Abacuza.JobRunners.Spark.SDK
{
    /// <summary>
    /// Represents the data that contains project and revision information.
    /// </summary>
    public sealed class ProjectContext
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <c>ProjectContext</c> class.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="projectName">The name of the project.</param>
        /// <param name="projectCreationDate">The date on which the project was created.</param>
        /// <param name="revisionId">The id of the project revision.</param>
        public ProjectContext(Guid projectId, string projectName, DateTime projectCreationDate, Guid revisionId)
            => (ProjectId, ProjectName, ProjectCreationDate, RevisionId) = (projectId, projectName, projectCreationDate, revisionId);

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the creation date of the project.
        /// </summary>
        public DateTime ProjectCreationDate { get; }

        /// <summary>
        /// Gets the id of the project.
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        public string ProjectName { get; }

        /// <summary>
        /// Gets the id of the revision.
        /// </summary>
        public Guid RevisionId { get; }

        #endregion Public Properties
    }
}