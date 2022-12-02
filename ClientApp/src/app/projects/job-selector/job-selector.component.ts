import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { BehaviorSubject, fromEvent, Observable, tap } from 'rxjs';
import { Category, Job } from 'src/app/_models/job.model';

@Component({
  selector: 'app-job-selector',
  templateUrl: './job-selector.component.html',
  styleUrls: ['./job-selector.component.css'],
})
export class JobSelectorComponent implements AfterViewInit {
  @Input() categories: Observable<Category[]>;
  @Input() jobs: Observable<Job[]>;
  jobList$: BehaviorSubject<Job[]> = new BehaviorSubject<Job[]>([]);
  @ViewChild('categoryInput') catInputRef: ElementRef;

  /**
   *
   */
  constructor() {}
  ngAfterViewInit(): void {
    fromEvent(this.catInputRef.nativeElement, 'change')
      .pipe(tap((e: any) => console.log('xD', e.target.value)))
      .subscribe();
  }

  // hElement: HTMLInputElement = this.elRef.NativeElement;
  onCategoryChange(selectedValue: string) {
    console.log('selected category:', selectedValue);

    // fromEvent(categoryInput)
  }
}
