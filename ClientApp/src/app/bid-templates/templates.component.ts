import { Component, OnInit } from '@angular/core';
import { ApiClientService } from '@services/api-client.service';
import { MessagesService } from '@services/messages.service';
import { TemplateService } from '@services/bid-template.service';
import { BehaviorSubject, finalize, Observable } from 'rxjs';
import { BidTemplate } from '../_models/bid-template';

@Component({
  selector: 'app-templates',
  templateUrl: './templates.component.html',
  styleUrls: ['./templates.component.css'],
})
export class TemplatesComponent implements OnInit {
  isToastVisible: boolean = false;
  templates$: Observable<BidTemplate[]> = this.templateService.templates$;
  toBeEdited$: BehaviorSubject<BidTemplate> = new BehaviorSubject<BidTemplate>(
    new BidTemplate()
  );
  constructor(
    private templateService: TemplateService,
    private messageService: MessagesService
  ) {}
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
  deleteTemplate(id: number) {
    this.templateService.removeTemplate(id).subscribe({
      next: () => this.messageService.addSuccess('Template Deleted!'),
    });
  }
  ngOnInit(): void {
    this.templateService.refreshTemplates().subscribe();
  }
}
