import { Component, Inject } from '@angular/core';
import {
  MatDialog,
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';
import { BidTemplate } from 'src/app/_models/bid-template';
@Component({
  selector: 'app-bid-template-dialog-form',
  templateUrl: './bid-template-dialog-form.component.html',
  styleUrls: ['./bid-template-dialog-form.component.css'],
})
export class BidTemplateDialogFormComponent {
  constructor(
    public dialogRef: MatDialogRef<BidTemplateDialogFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: BidTemplate
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
