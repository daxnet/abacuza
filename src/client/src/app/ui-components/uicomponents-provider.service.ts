import { Injectable } from '@angular/core';
import { CheckBoxComponent } from './check-box.component';
import { TextAreaComponent } from './text-area.component';
import { UIComponentItem } from './uicomponent-item';

@Injectable({
  providedIn: 'root'
})
export class UIComponentsProviderService {

  getRegisteredUIComponents(): UIComponentItem[] {
    return [
      new UIComponentItem('CheckBox', CheckBoxComponent),
      new UIComponentItem('TextArea', TextAreaComponent),
    ]
  }
}
