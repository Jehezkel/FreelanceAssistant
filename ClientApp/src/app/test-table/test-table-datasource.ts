import { DataSource } from '@angular/cdk/collections';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { map, tap } from 'rxjs/operators';
import { Observable, of as observableOf, merge, pipe } from 'rxjs';
import { ProjectSearchesService } from '@services/project-searches.service';
import { ProjectSearch } from '../_models/project-search';

/**
 * Data source for the TestTable view. This class should
 * encapsulate all logic for fetching and manipulating the displayed data
 * (including sorting, pagination, and filtering).
 */
export class TestTableDataSource extends DataSource<ProjectSearch> {
  data: any[] = [];
  paginator: MatPaginator | undefined;
  sort: MatSort | undefined;

  constructor(private searchService: ProjectSearchesService) {
    super();
    this.searchService.searches$
      .pipe(
        map((searches) =>
          searches.map((search) => ({
            ...search,
            jobs: search.input.jobs.map((j) => j.name).join(', '),
          }))
        ),
        tap((t) => console.log(t))
      )
      .subscribe((searches) => (this.data = searches));
  }

  /**
   * Connect this data source to the table. The table will only update when
   * the returned stream emits new items.
   * @returns A stream of the items to be rendered.
   */
  connect() {
    if (this.paginator && this.sort) {
      // Combine everything that affects the rendered data into one update
      // stream for the data-table to consume.
      return merge(
        // observableOf(this.data),
        this.searchService.searches$.asObservable(),
        this.paginator.page,
        this.sort.sortChange
      ).pipe(
        map(() => {
          return this.getPagedData(this.getSortedData([...this.data]));
        })
      );
    } else {
      throw Error(
        'Please set the paginator and sort on the data source before connecting.'
      );
    }
    // return this.searchService.searches$.asObservable();
  }

  /**
   *  Called when the table is being destroyed. Use this function, to clean up
   * any open connections or free any held resources that were set up during connect.
   */
  disconnect(): void {}
  private getModifiedStream() {
    return this.searchService.searches$.pipe(
      map((searches) =>
        searches.map((search) => ({
          ...search,
          jobs: search.input.jobs.map((j) => j.name).join(', '),
        }))
      ),
      tap((t) => console.log(t))
    );
  }
  /**
   * Paginate the data (client-side). If you're using server-side pagination,
   * this would be replaced by requesting the appropriate data from the server.
   */
  private getPagedData(data: ProjectSearch[]): ProjectSearch[] {
    if (this.paginator) {
      const startIndex = this.paginator.pageIndex * this.paginator.pageSize;
      return data.splice(startIndex, this.paginator.pageSize);
    } else {
      return data;
    }
  }

  /**
   * Sort the data (client-side). If you're using server-side sorting,
   * this would be replaced by requesting the appropriate data from the server.
   */
  private getSortedData(data: ProjectSearch[]): ProjectSearch[] {
    if (!this.sort || !this.sort.active || this.sort.direction === '') {
      return data;
    }

    return data.sort((a, b) => {
      const isAsc = this.sort?.direction === 'asc';
      switch (this.sort?.active) {
        case 'minPrice':
          return compare(a.input.minPrice ?? 0, b.input.minPrice ?? 0, isAsc);
        case 'id':
          return compare(+a.id, +b.id, isAsc);
        default:
          return 0;
      }
    });
  }
}

/** Simple sort comparator for example ID/Name columns (for client-side sorting). */
function compare(
  a: string | number,
  b: string | number,
  isAsc: boolean
): number {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
