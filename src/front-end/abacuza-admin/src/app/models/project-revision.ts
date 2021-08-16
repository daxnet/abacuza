export interface ProjectRevision {
    id: string;
    projectId: string;
    createdDate: Date;
    jobSubmissionName: string;
    jobStatusName: string;
    jobCancelledDate: Date;
    jobCompletedDate: Date;
    jobCreatedDate: Date;
    jobFailedDate: Date;
}
