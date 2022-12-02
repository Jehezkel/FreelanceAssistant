import { Component, OnInit } from '@angular/core';
import { ApiClientService } from '@services/api-client.service';
import { JobsService } from '@services/jobs.service';
import { Observable } from 'rxjs';
import { Category } from '../_models/job.model';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css'],
})
export class ProjectsComponent implements OnInit {
  response: any;
  categories$: Observable<Category[]>;
  constructor(private jobsService: JobsService) {
    this.categories$ = jobsService.categories$;
  }

  ngOnInit(): void {
    this.jobsService.refreshJobs().subscribe();
  }
}
