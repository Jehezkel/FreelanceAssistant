import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
})
export class TableComponent implements OnInit {
  @Input() Items$: Observable<any[]> = new Observable();
  Items: any[] = [];
  @Output() DeleteEmiter = new EventEmitter();
  @Output() EditEmiter = new EventEmitter();
  Columns: string[];
  constructor() {}
  ngOnInit(): void {
    this.Items$.subscribe((data) => {
      this.Items = data;
      if (data.length > 0) {
        console.log(data);
        this.Columns = Object.keys(this.Items[0]);
      }
    });
  }

  editItem(item: any) {
    this.EditEmiter.emit(item);
  }
  deleteItem(item: any) {
    this.DeleteEmiter.emit(item);
  }
}
