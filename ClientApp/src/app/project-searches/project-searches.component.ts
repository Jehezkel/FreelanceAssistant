import { Component, OnInit } from '@angular/core';
import { ProjectSearchesService } from '@services/project-searches.service';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { ProjectSearch, SearchInput } from '../_models/project-search';
import { ColOptions } from '../_shared/table/table.component';

@Component({
  selector: 'app-project-searches',
  templateUrl: './project-searches.component.html',
  styleUrls: ['./project-searches.component.css'],
})
export class ProjectSearchesComponent implements OnInit {
  searches$: any;
  tableOptions: ColOptions[] = [];
  constructor(private searchesService: ProjectSearchesService) {}
  ngOnInit(): void {
    this.searchesService.refreshSearches().subscribe();
  }
  OpenNewSearch() {}
}
