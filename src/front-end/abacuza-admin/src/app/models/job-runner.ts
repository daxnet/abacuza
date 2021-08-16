import { S3File } from "./s3-file";

export interface JobRunner {
    id: string;
    name: string;
    description: string;
    clusterType: string;
    payloadTemplate: string;
    payloadTemplateJsonObject: any;
    binaryFiles: S3File[];
}
