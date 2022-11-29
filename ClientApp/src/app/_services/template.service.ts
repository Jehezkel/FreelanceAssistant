import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { Template } from '../_models/template.model';
import { ApiClientService } from './api-client.service';

@Injectable({
  providedIn: 'root',
})
export class TemplateService {
  templates$: BehaviorSubject<Template[]> = new BehaviorSubject<Template[]>([]);

  constructor(private apiService: ApiClientService) {}
  refreshTemplates() {
    return this.apiService
      .getTemplates()
      .pipe(tap((data: Template[]) => this.templates$.next(data)));
  }
  addTemplate(desc: string) {
    return this.apiService
      .addTemplate(desc)
      .pipe(
        tap((data: Template) =>
          this.templates$.next([...this.templates$.value, data])
        )
      );
  }
  removeTemplate(id: number) {
    return this.apiService
      .removeTemplate(id)
      .pipe(
        tap(() =>
          this.templates$.next(this.templates$.value.filter((t) => t.id != id))
        )
      );
  }
  updateTemplate(template: Template) {
    return this.apiService.updateTemplate(template).pipe(
      tap((data: Template) => {
        var result = this.templates$.value;
        var templateIndex = result.findIndex((t) => t.id === template.id);
        result[templateIndex] = data;
        this.templates$.next(result);
      })
    );
  }
}
