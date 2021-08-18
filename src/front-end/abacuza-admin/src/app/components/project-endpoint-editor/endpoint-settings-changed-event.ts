import { Endpoint } from 'src/app/models/endpoint';

export class EndpointSettingsChangedEvent {
    constructor(public endpointDefinitionId: string, public endpoint: Endpoint, public component: string, public data: any) {
        
    }
}
