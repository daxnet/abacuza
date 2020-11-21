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
import { SmartTableDateCellRenderComponent } from 'app/components/smart-table-date-cell-render/smart-table-date-cell-render.component';
import { SmartTableJobStatusRenderComponent } from 'app/components/smart-table-job-status-render/smart-table-job-status-render.component';
import { TextMessageDialogComponent } from 'app/services/text-message-dialog/text-message-dialog.component';
import { ThemeModule } from 'app/@theme/theme.module';



@NgModule({
  declarations: [
    FileListComponent,
    CommonDialogComponent,
    TextMessageDialogComponent,
    FileUploadComponent,
    CheckBoxComponent,
    TextAreaComponent,
    DropDownBoxComponent,
    TextBoxComponent,
    FilePickerComponent,
    SmartTableDateCellRenderComponent,
    SmartTableJobStatusRenderComponent,],
  imports: [
    CommonModule,
    FormsModule,
    ThemeModule,
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
    TextMessageDialogComponent,
    FileUploadComponent,
    CheckBoxComponent,
    TextAreaComponent,
    DropDownBoxComponent,
    TextBoxComponent,
    FilePickerComponent,
    SmartTableDateCellRenderComponent,
  ]
})
export class SharedModule { }
