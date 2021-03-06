import { EventEmitter } from '@angular/core';
import { ComponentEvent } from './component-event';

export interface UIComponentBase {
    attributes: any;
    value: any;
    id: string;
    modelChange: EventEmitter<ComponentEvent>;
}
