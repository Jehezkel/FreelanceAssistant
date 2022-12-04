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
  Observable,
  of,
  pipe,
  startWith,
  switchMap,
  tap,
} from 'rxjs';
import { Category, Job } from 'src/app/_models/job.model';

@Component({
  selector: 'app-job-selector',
  templateUrl: './job-selector.component.html',
  styleUrls: ['./job-selector.component.css'],
})
export class JobSelectorComponent implements OnInit {
  @Input() categories: Category[];
  // jobs list filtered by category at smart component
  @Input() jobs: Job[];
  @Output() selectedCategory = new EventEmitter<string>();
  //Search box
  @ViewChild('jobSearchBox') jobSearchBoxRef: ElementRef;
  jobs$: Observable<Job[]>;
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
      debounceTime(300)
    );
    console.log('CHUJ', this.jobs);
    this.jobs$ = of(this.jobs).pipe(
      concatMap(
        () => this.searchInput$,
        (ja: Job[], input: string) => {
          console.log('jobs ', ja);
          console.log('input: ', input);
          return ja.filter((j) => j.name.indexOf(input) != -1);
        }
      ),
      tap((x: any) => console.log('asd', x))
    );
    this.jobs$.subscribe();
  }
  ngAfterViewInit(): void {}

  // hElement: HTMLInputElement = this.elRef.NativeElement;
  onCategoryChange(selectedValue: string) {
    console.log('selected category:', selectedValue);
    this.selectedCategory.emit(selectedValue);
    console.log('CHUJ', this.jobs);
    // fromEvent(categoryInput)
  }
}
