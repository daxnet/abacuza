import { ProjectEndpointDefinition } from './project-endpoint-definition';

export interface Project {
    id?: string;
    name: string;
    description: string;
    dateCreated?: Date;
    jobRunnerId: string;
    jobRunnerName?: string;
    inputEndpoints: ProjectEndpointDefinition[];
    outputEndpoint: ProjectEndpointDefinition;
    inputEndpointUIComponentData?: any;
    outputEndpointUIComponentData?: any;
}
