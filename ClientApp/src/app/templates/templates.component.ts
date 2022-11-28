import { Component, OnInit } from '@angular/core';
import { ApiClientService } from '@services/api-client.service';
import { MessagesService } from '@services/messages.service';
import { TemplateService } from '@services/template.service';
import { BehaviorSubject, finalize, Observable } from 'rxjs';
import { Template } from '../_models/template.model';

@Component({
  selector: 'app-templates',
  templateUrl: './templates.component.html',
  styleUrls: ['./templates.component.css'],
})
export class TemplatesComponent implements OnInit {
  isToastVisible: boolean = false;
  templates$: Observable<Template[]> = this.templateService.templates$;
  toBeEdited$: BehaviorSubject<Template> = new BehaviorSubject<Template>(
    new Template()
  );
  constructor(
    private templateService: TemplateService,
    private messageService: MessagesService
  ) {}
  onToastSave(template: Template) {
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
  editTemplate(template: Template) {
    this.toBeEdited$.next(template);
    this.isToastVisible = true;
  }
  addTemplate() {
    this.toBeEdited$.next(new Template());
    this.isToastVisible = true;
  }
  deleteTemplate(id: number) {
    this.templateService
      .removeTemplate(id)
      .subscribe({
        next: () => this.messageService.addSuccess('Template Deleted!'),
      });
  }
  ngOnInit(): void {
    this.templateService.refreshTemplates().subscribe();
  }
}
