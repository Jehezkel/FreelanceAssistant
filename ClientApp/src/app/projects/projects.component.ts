import { Component, OnInit } from '@angular/core';
import { ApiClientService } from '@services/api-client.service';
import { JobsService } from '@services/jobs.service';
import {
  BehaviorSubject,
  distinct,
  Observable,
  switchMap,
  map,
  mergeMap,
} from 'rxjs';
import { Category, Job } from '../_models/job.model';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css'],
})
export class ProjectsComponent implements OnInit {
  response: any;
  categories$: Observable<Category[]>;
  jobs$: BehaviorSubject<Job[]> = new BehaviorSubject<Job[]>([] as Job[]);
  selectedCategoryName$ = new BehaviorSubject<string>('Any');
  constructor(private jobsService: JobsService) {
    this.categories$ = this.jobsService.categories$;
  }

  ngOnInit(): void {
    this.jobsService.refreshJobs().subscribe();
    this.selectedCategoryName$
      .pipe(
        switchMap((cat_name: string) =>
          this.jobsService.jobs$.pipe(
            map((j: Job[]) => {
              if (cat_name === 'Any') return j;
              else return j.filter((i) => i.category.name === cat_name);
            })
          )
        )
      )
      .subscribe(this.jobs$);
  }
  filterJobsByCategory(categoryName: string) {
    this.selectedCategoryName$.next(categoryName);
  }
  onJobClick(job: Job) {
    var afterChange = this.jobs$.value;
    var index = afterChange.findIndex((j) => j.id == job.id);
    afterChange[index] = job;
    this.jobs$.next(afterChange);
  }
}
