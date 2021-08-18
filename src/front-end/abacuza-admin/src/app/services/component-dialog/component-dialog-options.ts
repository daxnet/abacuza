
export enum ComponentDialogUsage {
    Create,
    Edit
}

export class ComponentDialogOptions {
    public title?: string;
    public acceptButtonText?: string;
    public cancelButtonText?: string;
    public usage?: ComponentDialogUsage;

    constructor() {
    }


    static createDefault(): ComponentDialogOptions {
        return {
            title: 'Title',
            acceptButtonText: 'Save',
            cancelButtonText: 'Cancel',
            usage: ComponentDialogUsage.Create
        };
    }
}