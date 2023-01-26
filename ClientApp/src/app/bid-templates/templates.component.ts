import { Component, OnInit } from '@angular/core';
import { ApiClientService } from '@services/api-client.service';
import { MessagesService } from '@services/messages.service';
import { TemplateService } from '@services/bid-template.service';
import { BehaviorSubject, finalize, Observable } from 'rxjs';
import { BidTemplate } from '../_models/bid-template';
import { MatTableDataSource } from '@angular/material/table';
import { TemplateDataSource } from '@services/template.dataSource';

@Component({
  selector: 'app-templates',
  templateUrl: './templates.component.html',
  styleUrls: ['./templates.component.css'],
})
export class TemplatesComponent implements OnInit {
  isToastVisible: boolean = false;
  // dataSource: TemplateDataSource = new TemplateDataSource(this.templateService);
  dataSource = new MatTableDataSource<BidTemplate>();
  templates$ = this.templateService.templates$;
  toBeEdited$: BehaviorSubject<BidTemplate> = new BehaviorSubject<BidTemplate>(
    new BidTemplate()
  );
  displayedColumns = ['id', 'description', 'actions'];
  constructor(
    private templateService: TemplateService,
    private messageService: MessagesService
  ) {
    this.templateService.templates$.subscribe(
      (data) => (this.dataSource.data = data)
    );
    // this.dataSource = new TemplateDataSource(this.templateService);
    // this.dataSource = new MatTableDataSource<BidTemplate>(this.templates);
    // this.templates$.subscribe((data) => (this.dataSource.data = data));
  }
  onToastSave(template: BidTemplate) {
    if (template.id) {
      this.templateService
        .updateTemplate(template)
        .pipe(finalize(() => (this.isToastVisible = false)))
        .subscribe({
          next: () => this.messageService.addSuccess('Template Updated'),
          error: (err) =>
            this.messageService.addError(`Error occured {err}`, 10),
        });
    } else {
      this.templateService
        .addTemplate(template.description)
        .pipe(finalize(() => (this.isToastVisible = false)))
        .subscribe({
          next: () => this.messageService.addSuccess('New Template Added'),
          error: (err) =>
            this.messageService.addError(`Error occured {err}`, 10),
        });
    }
  }
  editTemplate(template: BidTemplate) {
    this.toBeEdited$.next(template);
    this.isToastVisible = true;
  }
  addTemplate() {
    this.toBeEdited$.next(new BidTemplate());
    this.isToastVisible = true;
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
