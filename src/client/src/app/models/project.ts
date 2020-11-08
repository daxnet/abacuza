export class Project {
    id: string;
    name: string;
    description: string;
    dateCreated: Date;
    jobRunnerId: string;
    jobRunnerName?: string;
    inputEndpointName: string;
    inputEndpointDisplayName: string;
    inputEndpointSettings: string;
    uiComponentData: any;
}
