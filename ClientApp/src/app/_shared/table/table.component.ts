import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { first, map } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
})
export class TableComponent implements OnInit {
  @Input() Items$: Observable<any[]> = new Observable();
  @Input() TableOptions: ColOptions[] | undefined;
  Items: any[] = [];
  @Output() DeleteEmiter = new EventEmitter();
  @Output() EditEmiter = new EventEmitter();
  Columns: string[];
  // test: number = this.TableOptions?.length
  constructor() {}
  ngOnInit(): void {
    if (!this.TableOptions) {
      this.Items$.pipe(
        first((data) => data.length > 0),
        map((data) => Object.keys(data[0]).map((val) => new ColOptions(val)))
      ).subscribe((rowOptions) => (this.TableOptions = rowOptions));
    }
  }

  editItem(item: any) {
    this.EditEmiter.emit(item);
  }
  deleteItem(item: any) {
    this.DeleteEmiter.emit(item);
  }
}
export class ColOptions {
  private _headerName: string;
  propName: string;
  set headerName(value: string) {
    this._headerName = value;
  }
  get headerName(): string {
    return this._headerName ?? this.propName;
  }
  defaultValue: string = '';
  numberFormat?: string;
  constructor(propName: string) {
    this.propName = propName;
  }
}
