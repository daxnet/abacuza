import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { MainSideBarComponent } from './components/main-side-bar/main-side-bar.component';
import { FooterBarComponent } from './components/footer-bar/footer-bar.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import { InstalledPluginsComponent } from './pages/installed-plugins/installed-plugins.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpAuthInterceptorService } from './services/http-auth-interceptor.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DataTablesModule } from 'angular-datatables';
import { ClusterConnectionsComponent } from './pages/cluster-connections/cluster-connections.component';
import { CommonDialogComponent } from './services/common-dialog/common-dialog.component';
import { ToastsComponent } from './services/toast/toasts.component';
import { EditClusterConnectionComponent } from './pages/edit-cluster-connection/edit-cluster-connection.component';
import { ComponentDialogComponent } from './services/component-dialog/component-dialog.component';
import { ComponentDialogContentDirective } from './services/component-dialog/component-dialog-content.directive';
import { FormsModule } from '@angular/forms';
import { NgJsonEditorModule } from 'ang-jsoneditor';
import { JobRunnersComponent } from './pages/job-runners/job-runners.component';
import { JobRunnerDetailsComponent } from './pages/job-runner-details/job-runner-details.component';
import { FileListComponent } from './components/file-list/file-list.component';
import { FileUploadComponent } from './services/file-upload/file-upload.component';
import { CreateJobRunnerComponent } from './pages/create-job-runner/create-job-runner.component';
import { ProjectsComponent } from './pages/projects/projects.component';
import { CreateProjectComponent } from './pages/create-project/create-project.component';
import { ProjectDetailsComponent } from './pages/project-details/project-details.component';
import { ProjectEndpointEditorComponent } from './components/project-endpoint-editor/project-endpoint-editor.component';
import { EndpointEditorHostDirective } from './components/project-endpoint-editor/endpoint-editor-host.directive';
import { TextBoxComponent } from './components/ui-components/text-box.component';
import { TextAreaComponent } from './components/ui-components/text-area.component';
import { CheckBoxComponent } from './components/ui-components/check-box.component';
import { DropDownBoxComponent } from './components/ui-components/drop-down-box.component';
import { JsonTextAreaComponent } from './components/ui-components/json-text-area.component';
import { FilePickerComponent } from './components/ui-components/file-picker.component';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { SmartTableDateCellRenderComponent } from './components/smart-table-date-cell-render/smart-table-date-cell-render.component';
import { SmartTableJobStatusRenderComponent } from './components/smart-table-job-status-render/smart-table-job-status-render.component';
import { TextMessageAreaComponent } from './components/text-message-area/text-message-area.component';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    MainSideBarComponent,
    FooterBarComponent,
    DashboardComponent,
    NotFoundComponent,
    AuthCallbackComponent,
    InstalledPluginsComponent,
    ClusterConnectionsComponent,
    CommonDialogComponent,
    ToastsComponent,
    EditClusterConnectionComponent,
    ComponentDialogComponent,
    ComponentDialogContentDirective,
    JobRunnersComponent,
    JobRunnerDetailsComponent,
    FileListComponent,
    FileUploadComponent,
    CreateJobRunnerComponent,
    ProjectsComponent,
    CreateProjectComponent,
    ProjectDetailsComponent,
    ProjectEndpointEditorComponent,
    EndpointEditorHostDirective,
    TextBoxComponent,
    TextAreaComponent,
    CheckBoxComponent,
    DropDownBoxComponent,
    JsonTextAreaComponent,
    FilePickerComponent,
    SmartTableDateCellRenderComponent,
    SmartTableJobStatusRenderComponent,
    TextMessageAreaComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
    DataTablesModule,
    NgJsonEditorModule,
    Ng2SmartTableModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpAuthInterceptorService,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
