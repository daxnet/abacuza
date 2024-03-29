import { ProjectEndpointDefinition } from './project-endpoint-definition';

export interface Project {
    id?: string;
    name: string;
    description: string;
    dateCreated?: Date;
    jobRunnerId: string;
    jobRunnerName?: string;
    inputEndpoints: ProjectEndpointDefinition[];
    selectedOutputEndpointId: string;
    outputEndpoints: ProjectEndpointDefinition[];
}
