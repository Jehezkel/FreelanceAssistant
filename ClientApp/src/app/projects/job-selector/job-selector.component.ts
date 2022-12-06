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
      <button
        [ngClass]="{ 'bg-sky-500': job.selected }"
        class="p-2"
        *ngFor="let job of filteredJobs$ | async"
        (click)="onJobClick(job)"
      >
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
  @Output() clickedJob = new EventEmitter<Job>();
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
      startWith(''),
      filter(
        (search_val: string) => search_val.length > 2 || search_val.length === 0
      ),
      distinctUntilChanged(),
      debounceTime(300)
    );
    this.filteredJobs$ = this.searchInput$.pipe(
      switchMap((input) => {
        if (input.length > 0) {
          return this.jobs$.pipe(
            map((jobs) =>
              jobs.filter(
                (j) => j.name.toUpperCase().indexOf(input.toUpperCase()) != -1
              )
            )
          );
        } else {
          return this.jobs$;
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
  onJobClick(job: Job) {
    job.selected = !job.selected;
    this.clickedJob.emit(job);
  }
}
