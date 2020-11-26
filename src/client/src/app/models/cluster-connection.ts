export class ClusterConnection {
    clusterType: string;
    description: string;
    id: string;
    name: string;
    settings: string;

    /**
     * Represents the JSON object that reflects the value of settings.
     *
     * @type {*}
     * @memberof ClusterConnection
     */
    settingsJsonObject: any;

    constructor() {
        this.settingsJsonObject = {};
    }
}
