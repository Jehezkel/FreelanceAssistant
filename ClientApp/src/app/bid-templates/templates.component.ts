import { Component, OnInit } from '@angular/core';
import { ApiClientService } from '@services/api-client.service';
import { MessagesService } from '@services/messages.service';
import { TemplateService } from '@services/bid-template.service';
import { BehaviorSubject, finalize, Observable } from 'rxjs';
import { BidTemplate } from '../_models/bid-template';
import { MatTableDataSource } from '@angular/material/table';
import { TemplateDataSource } from '@services/template.dataSource';
import { ColOptions } from '../_shared/table/table.component';

@Component({
  selector: 'app-templates',
  templateUrl: './templates.component.html',
  styleUrls: ['./templates.component.css'],
})
export class TemplatesComponent implements OnInit {
  isTemplateFormVisible: boolean = false;
  templates$ = this.templateService.templates$;
  templateInput = new BidTemplate();
  tableOptions: ColOptions[] = [];
  constructor(
    private templateService: TemplateService,
    private messageService: MessagesService
  ) {
    this.configureTable();
  }
  OnDialogSave(template: BidTemplate) {
    if (template.id) {
      this.templateService
        .updateTemplate(template)
        .pipe(finalize(() => (this.isTemplateFormVisible = false)))
        .subscribe({
          next: () => this.messageService.addSuccess('Template Updated'),
          error: (err) =>
            this.messageService.addError(`Error occured {err}`, 10),
        });
    } else {
      this.templateService
        .addTemplate(template.description)
        .pipe(finalize(() => (this.isTemplateFormVisible = false)))
        .subscribe({
          next: () => this.messageService.addSuccess('New Template Added'),
          error: (err) =>
            this.messageService.addError(`Error occured {err}`, 10),
        });
    }
  }
  editTemplate(template: BidTemplate) {
    console.log('pushed val ', template);
    // this.templateInput$.next(template);
    this.templateInput = template;
    // console.log('subj value', this.templateInput$.value);
    this.isTemplateFormVisible = true;
  }
  addTemplate() {
    // this.templateInput$.next(new BidTemplate());
    this.templateInput = new BidTemplate();
    this.isTemplateFormVisible = true;
  }
  deleteTemplate(template: BidTemplate) {
    this.templateService.removeTemplate(template.id!).subscribe({
      next: () => this.messageService.addSuccess('Template Deleted!'),
    });
  }
  ngOnInit(): void {
    this.templateService.refreshTemplates().subscribe();
  }
  configureTable() {
    let idCol = new ColOptions('id');
    idCol.headerName = 'ID';
    idCol.numberFormat = '.2';
    this.tableOptions.push(idCol);

    let descCol = new ColOptions('description');
    descCol.headerName = 'Description';
    this.tableOptions.push(descCol);

    let userCol = new ColOptions('userId');
    userCol.headerName = 'User';
    userCol.defaultValue = '-';
    this.tableOptions.push(userCol);
  }
}
