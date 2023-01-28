import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css'],
})
export class DialogComponent {
  @Input() isDialogVisible: boolean = false;
  @Output() isDialogVisibleChange: EventEmitter<boolean> = new EventEmitter();
  togglVisible() {
    this.isDialogVisible = !this.isDialogVisible;
    this.isDialogVisibleChange.emit(this.isDialogVisible);
  }
}
