import { Injectable, Type } from '@angular/core';
import { CheckBoxComponent } from './check-box.component';
import { TextBoxComponent } from './text-box.component';
import { TextAreaComponent } from './text-area.component';
import { UIComponentBase } from './uicomponent-base';

@Injectable({
  providedIn: 'root'
})
export class UIComponentProviderService {

  constructor() { }

  getRegisteredUIComponents(): {name: string, component: Type<any>}[] {
    return [
      {
        name: 'TextBox',
        component: TextBoxComponent
      },
      {
        name: 'CheckBox',
        component: CheckBoxComponent
      },
      {
        name: 'TextArea',
        component: TextAreaComponent
      }
    ];
  }
}
