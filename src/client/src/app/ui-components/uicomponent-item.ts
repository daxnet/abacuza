import { Type } from '@angular/core';

export class UIComponentItem {
    constructor(public name: string, public component: Type<any>) { }
}
