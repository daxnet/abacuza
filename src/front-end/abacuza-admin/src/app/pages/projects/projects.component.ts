import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subject, Subscription } from 'rxjs';
import { Project } from 'src/app/models/project';
import { ProjectsService } from 'src/app/services/projects.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectsComponent implements OnInit, OnDestroy {

  dtTableOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  projects: Project[] | null = null;
  
  private subscriptions: Subscription[] = [];

  @ViewChild(DataTableDirective, { static: false })
  dtElement!: DataTableDirective;
  
  constructor(private projectsService: ProjectsService) { }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    this.subscriptions?.forEach(s => s.unsubscribe());
  }

  ngOnInit(): void {
    this.dtTableOptions = {
      responsive: true,
      pagingType: 'full_numbers'
    };

    this.subscriptions.push(this.projectsService.getProjects()
      .subscribe(response => {
        this.projects = response.body;
        this.dtTrigger.next();
      }));
  }

  private rerender(): void {
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.destroy();
      this.dtTrigger.next();
    });
  }

}
