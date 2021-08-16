
/**
 * Represents the endpoint model.
 *
 * @export
 * @interface Endpoint
 */
export interface Endpoint {
    name: string;
    displayName: string;
    description: string;
    type: number;
    typeName: string;
    configurationUIElements: any[];
}
