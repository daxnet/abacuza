import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileListComponent } from 'app/components/file-list/file-list.component';
import { CommonDialogComponent } from 'app/services/common-dialog/common-dialog.component';
import { FileUploadComponent } from 'app/services/file-upload/file-upload.component';
import { CheckBoxComponent } from 'app/ui-components/check-box.component';
import { DropDownBoxComponent } from 'app/ui-components/drop-down-box.component';
import { FilePickerComponent } from 'app/ui-components/file-picker.component';
import { TextAreaComponent } from 'app/ui-components/text-area.component';
import { TextBoxComponent } from 'app/ui-components/text-box.component';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { NbIconModule, NbInputModule, NbCardModule, NbButtonModule, NbCheckboxModule, NbSelectModule, NbToastrModule, NbTooltipModule, NbProgressBarModule, NbDialogModule, NbMenuModule, NbSidebarModule } from '@nebular/theme';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    FileListComponent,
    CommonDialogComponent,
    FileUploadComponent,
    CheckBoxComponent,
    TextAreaComponent,
    DropDownBoxComponent,
    TextBoxComponent,
    FilePickerComponent,],
  imports: [
    CommonModule,
    FormsModule,
    Ng2SmartTableModule,
    NbEvaIconsModule,
    NbIconModule,
    NbInputModule,
    NbCardModule,
    NbButtonModule,
    NbCheckboxModule,
    NbSelectModule,
    NbToastrModule,
    NbTooltipModule,
    NbProgressBarModule,
    NbSidebarModule.forRoot(),
    NbMenuModule.forRoot(),
    NbDialogModule.forRoot(),
  ],
  exports: [
    FileListComponent,
    CommonDialogComponent,
    FileUploadComponent,
    CheckBoxComponent,
    TextAreaComponent,
    DropDownBoxComponent,
    TextBoxComponent,
    FilePickerComponent,
  ]
})
export class SharedModule { }
