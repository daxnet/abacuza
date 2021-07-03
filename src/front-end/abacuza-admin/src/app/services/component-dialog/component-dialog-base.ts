import { EventEmitter } from "@angular/core";
import { ComponentDialogEvent } from "./component-dialog-event";
import { ComponentDialogUsage } from "./component-dialog-options";

export interface ComponentDialogBase {
    usage?: ComponentDialogUsage;
    data: any;
}
