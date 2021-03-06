import { Injectable } from '@angular/core';
import { CheckBoxComponent } from './check-box.component';
import { DropDownBoxComponent } from './drop-down-box.component';
import { FilePickerComponent } from './file-picker.component';
import { JsonTextAreaComponent } from './json-text-area.component';
import { TextAreaComponent } from './text-area.component';
import { TextBoxComponent } from './text-box.component';
import { UIComponentItem } from './uicomponent-item';

@Injectable({
  providedIn: 'root',
})
export class UIComponentsProviderService {

  getRegisteredUIComponents(): UIComponentItem[] {
    return [
      new UIComponentItem('Checkbox', CheckBoxComponent),
      new UIComponentItem('TextArea', TextAreaComponent),
      new UIComponentItem('DropDownBox', DropDownBoxComponent),
      new UIComponentItem('TextBox', TextBoxComponent),
      new UIComponentItem('FilePicker', FilePickerComponent),
      new UIComponentItem('JsonTextArea', JsonTextAreaComponent),
    ];
  }
}
