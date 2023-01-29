import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { BidTemplate } from '../_models/bid-template';
import { ApiClientService } from './api-client.service';

@Injectable({
  providedIn: 'root',
})
export class TemplateService {
  templates$ = new BehaviorSubject<BidTemplate[]>([]);

  constructor(private apiService: ApiClientService) {}
  refreshTemplates() {
    return this.apiService
      .getTemplates()
      .pipe(tap((data: BidTemplate[]) => this.templates$.next(data)));
  }
  addTemplate(template: BidTemplate) {
    return this.apiService
      .addTemplate(template)
      .pipe(
        tap((data: BidTemplate) =>
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
  updateTemplate(template: BidTemplate) {
    return this.apiService.updateTemplate(template).pipe(
      tap((data: BidTemplate) => {
        var result = this.templates$.value;
        var templateIndex = result.findIndex((t) => t.id === template.id);
        result[templateIndex] = data;
        this.templates$.next(result);
      })
    );
  }
}
