import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { EventType } from '@angular/router';
import {
  BehaviorSubject,
  concatMap,
  debounce,
  debounceTime,
  distinctUntilChanged,
  filter,
  fromEvent,
  map,
  mergeMap,
  mergeMapTo,
  Observable,
  of,
  pipe,
  startWith,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import { Category, Job } from 'src/app/_models/job.model';

@Component({
  selector: 'app-job-selector',
  // templateUrl: './job-selector.component.html',
  template: `<select
      (change)="onCategoryChange(categoryInput.value)"
      #categoryInput
      class="outline-none"
    >
      <option>Any</option>
      <option *ngFor="let category of categories">
        {{ category.name }}
      </option>
    </select>
    <div><input #jobSearchBox type="text" name="" id="" /></div>
    <div>
      <button class="p-2" *ngFor="let job of filteredJobs$ | async">
        {{ job.name }}
      </button>
    </div> `,
  styleUrls: ['./job-selector.component.css'],
})
export class JobSelectorComponent implements OnInit {
  @Input() categories: Category[];
  // jobs list filtered by category at smart component
  @Input() jobs$: Observable<Job[]>;
  @Output() selectedCategory = new EventEmitter<string>();
  //Search box
  @ViewChild('jobSearchBox') jobSearchBoxRef: ElementRef;
  filteredJobs$: Observable<Job[]>;
  searchInput$: Observable<string>;
  constructor(private el: ElementRef) {}
  ngOnInit(): void {
    this.searchInput$ = fromEvent<InputEvent>(
      this.el.nativeElement,
      'keyup'
    ).pipe(
      map((event) => (event.target as HTMLInputElement).value),
      filter((search_val: string) => search_val.length > 2),
      distinctUntilChanged(),
      debounceTime(300),
      startWith('')
    );
    this.filteredJobs$ = this.jobs$.pipe(
      withLatestFrom(this.searchInput$),
      map(([jobs, input]) => {
        if (input.length > 0) {
          return jobs.filter((j) => j.name.indexOf(input) != -1);
        } else {
          return jobs;
        }
      })
    );
    // this.jobs$.subscribe();
  }
  ngAfterViewInit(): void {}

  // hElement: HTMLInputElement = this.elRef.NativeElement;
  onCategoryChange(selectedValue: string) {
    console.log('selected category:', selectedValue);
    this.selectedCategory.emit(selectedValue);
  }
}
