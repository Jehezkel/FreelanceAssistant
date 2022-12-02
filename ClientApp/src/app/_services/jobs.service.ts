import { Injectable } from '@angular/core';
import {
  BehaviorSubject,
  distinct,
  map,
  mergeMap,
  Observable,
  tap,
  toArray,
} from 'rxjs';
import { Category, Job } from '../_models/job.model';
import { ApiClientService } from './api-client.service';

@Injectable({
  providedIn: 'root',
})
export class JobsService {
  categories$: Observable<Category[]>;
  jobs$: BehaviorSubject<Job[]> = new BehaviorSubject<Job[]>([]);
  constructor(private apiService: ApiClientService) {
    this.categories$ = this.jobs$.pipe(
      map((data: Job[]) => {
        var result = [new Category()];
        data.forEach((j) =>
          result.findIndex((cat) => cat.id === j.category.id) === -1
            ? result.push(j.category)
            : null
        );
        return result;
      })
    );
  }
  refreshJobs() {
    return this.apiService
      .fl_getJobs()
      .pipe(tap((data: Job[]) => this.jobs$.next(data)));
  }
}
