import { Component, OnInit, ViewChild } from '@angular/core';
import { ApiClientService } from '@services/api-client.service';
import { MessagesService } from '@services/messages.service';
import { TemplateService } from '@services/bid-template.service';
import { BehaviorSubject, filter, finalize, map, Observable } from 'rxjs';
import { BidTemplate } from '../_models/bid-template';
import { TemplateDataSource } from '@services/template.dataSource';
import { ColOptions } from '../_shared/table/table.component';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { BidTemplateDialogFormComponent } from './bid-template-dialog-form/bid-template-dialog-form.component';

@Component({
  selector: 'app-templates',
  templateUrl: './templates.component.html',
  styleUrls: ['./templates.component.css'],
})
export class TemplatesComponent implements OnInit {
  @ViewChild(MatTable) table!: MatTable<BidTemplate>;
  isTemplateFormVisible: boolean = false;
  templates$ = this.templateService.templates$;
  templateInput = new BidTemplate();
  tableOptions: ColOptions[] = [];
  displayedColumns = ['id', 'desc', 'user', 'actions'];
  // private dataSource = new MatTableDataSource<BidTemplate>();
  // thingsAsMatTableDataSource$: Observable<MatTableDataSource<BidTemplate>> =
  //   this.templateService.templates$.pipe(
  //     map((things) => {
  //       const dataSource = this.dataSource;
  //       dataSource.data = things;
  //       return dataSource;
  //     })
  //   );
  constructor(
    private templateService: TemplateService,
    private messageService: MessagesService,
    private dialog: MatDialog
  ) {
    // this.configureTable();
  }

  editTemplate(template: BidTemplate) {
    // console.log('pushed val ', template);
    // // this.templateInput$.next(template);
    // this.templateInput = template;
    // // console.log('subj value', this.templateInput$.value);
    // this.isTemplateFormVisible = true;
    this.showDialog(template)
      .afterClosed()
      .pipe(filter((result) => result))
      .subscribe((result) => {
        console.log('afterclosed result: ', result);
        this.templateService.updateTemplate(result).subscribe({
          next: () => this.messageService.addSuccess('Template Updated'),
          error: (err) =>
            this.messageService.addError(`Error occured {err}`, 10),
        });
      });
  }
  showDialog(template?: BidTemplate) {
    const dialogRef = this.dialog.open(BidTemplateDialogFormComponent, {
      data: template ?? new BidTemplate(),
    });
    return dialogRef;
  }
  addTemplate() {
    // this.templateInput$.next(new BidTemplate());
    this.showDialog()
      .afterClosed()
      .pipe(filter((result) => result))
      .subscribe((result) => {
        console.log('afterclosed result: ', result);
        this.templateService.addTemplate(result).subscribe({
          next: () => this.messageService.addSuccess('New Template Added'),
          error: (err) =>
            this.messageService.addError(`Error occured {err}`, 10),
        });
      });
    // this.templateInput = new BidTemplate();
    // this.isTemplateFormVisible = true;
  }
  deleteTemplate(template: BidTemplate) {
    this.templateService.removeTemplate(template.id!).subscribe({
      next: () => this.messageService.addSuccess('Template Deleted!'),
    });
  }
  ngOnInit(): void {
    this.templateService.refreshTemplates().subscribe();
  }
}
