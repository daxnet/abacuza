import { S3File } from './s3-file';

export class JobRunner {
    id: string;
    name: string;
    description: string;
    clusterType: string;
    payloadTemplate: string;
    binaryFiles: S3File[];
}
