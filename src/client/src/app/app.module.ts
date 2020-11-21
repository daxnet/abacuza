/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { CoreModule } from './@core/core.module';
import { ThemeModule } from './@theme/theme.module';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { CommonDialogComponent } from './services/common-dialog/common-dialog.component';
import { CommonDialogService } from './services/common-dialog.service';
import { FileUploadComponent } from './services/file-upload/file-upload.component';
import { FileUploadService } from './services/file-upload.service';
import { CheckBoxComponent } from './ui-components/check-box.component';
import { TextAreaComponent } from './ui-components/text-area.component';
import { DropDownBoxComponent } from './ui-components/drop-down-box.component';
import { TextBoxComponent } from './ui-components/text-box.component';
import { FormsModule } from '@angular/forms';
import { FileListComponent } from './components/file-list/file-list.component';
import { FilePickerComponent } from './ui-components/file-picker.component';
import { SharedModule } from './shared/shared.module';
import { TextMessageDialogComponent } from './services/text-message-dialog/text-message-dialog.component';
import { TextMessageDialogService } from './services/text-message-dialog.service';

@NgModule({
  declarations: [
    AppComponent,
    ],
  imports: [
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    SharedModule,
    CoreModule.forRoot(),
    ThemeModule.forRoot(),
  ],
  bootstrap: [AppComponent],
  providers: [CommonDialogService, FileUploadService, TextMessageDialogService,],
  entryComponents: [
    CommonDialogComponent,
    TextMessageDialogComponent,
    FileUploadComponent,
    CheckBoxComponent,
    TextAreaComponent,
    DropDownBoxComponent,
    TextBoxComponent,
    FilePickerComponent,
    FileListComponent,
  ],
})
export class AppModule {
}
