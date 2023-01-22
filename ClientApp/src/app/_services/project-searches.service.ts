import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { ProjectSearch } from '../_models/project-search';
import { ApiClientService } from './api-client.service';

@Injectable({
  providedIn: 'root',
})
export class ProjectSearchesService {
  searches$ = new BehaviorSubject<ProjectSearch[]>([]);
  constructor(private apiService: ApiClientService) {}
  refreshSearches() {
    return this.apiService
      .getSearches()
      .pipe(tap((data: ProjectSearch[]) => this.searches$.next(data)));
  }
  addSearch(projSearch: ProjectSearch) {
    return this.apiService
      .addSearch(projSearch)
      .pipe(
        tap((data: ProjectSearch) =>
          this.searches$.next([...this.searches$.value, data])
        )
      );
  }
  updateSearch(projSearch: ProjectSearch) {
    return this.apiService
      .updateSearch(projSearch)
      .pipe(
        tap((data: ProjectSearch) =>
          this.searches$.next([
            ...this.searches$.value.filter((s) => s.id != projSearch.id),
            data,
          ])
        )
      );
  }
  removeSearch(id: number) {
    return this.apiService
      .removeSearch(id)
      .pipe(
        tap(() =>
          this.searches$.next(this.searches$.value.filter((s) => s.id != id))
        )
      );
  }
}
