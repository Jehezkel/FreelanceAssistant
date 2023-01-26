import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BidTemplate } from '../_models/bid-template';
import { TemplateService } from './bid-template.service';

@Injectable()
export class TemplateDataSource extends DataSource<BidTemplate> {
  constructor(private templateService: TemplateService) {
    super();
  }
  connect() {
    return this.templateService.templates$;
  }
  disconnect(): void {}
}
