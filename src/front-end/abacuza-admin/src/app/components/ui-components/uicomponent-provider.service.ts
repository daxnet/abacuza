import { Injectable, Type } from '@angular/core';
import { CheckBoxComponent } from './check-box.component';
import { TextBoxComponent } from './text-box.component';
import { TextAreaComponent } from './text-area.component';
import { UIComponentBase } from './uicomponent-base';
import { DropDownBoxComponent } from './drop-down-box.component';
import { JsonTextAreaComponent } from './json-text-area.component';
import { FilePickerComponent } from './file-picker.component';

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
        name: 'Checkbox',
        component: CheckBoxComponent
      },
      {
        name: 'TextArea',
        component: TextAreaComponent
      },
      {
        name: 'DropDownBox',
        component: DropDownBoxComponent
      },
      {
        name: 'JsonTextArea',
        component: JsonTextAreaComponent
      },
      {
        name: 'FilePicker',
        component: FilePickerComponent
      }
    ];
  }
}
