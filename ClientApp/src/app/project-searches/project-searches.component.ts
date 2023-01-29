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
    this.prepareList();
    this.configureTable();
  }

  configureTable() {
    let idCol = new ColOptions('ID');
    idCol.headerName = 'ID';
    // idCol.numberFormat = '.2';
    this.tableOptions.push(idCol);

    let minPrice = new ColOptions('PriceRange');
    minPrice.headerName = 'Price Range';
    this.tableOptions.push(minPrice);

    let jobs = new ColOptions('Jobs');
    jobs.headerName = 'Jobs';
    this.tableOptions.push(jobs);

    let userCol = new ColOptions('userName');
    userCol.headerName = 'User';
    userCol.defaultValue = '-';
    this.tableOptions.push(userCol);
  }
  prepareList() {
    const formatter = new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
    });
    this.searches$ = this.searchesService.searches$.pipe(
      map((data) =>
        data.map((el) => ({
          ID: el.id,
          PriceRange: `${formatter.format(
            el.input.minPrice ?? 0
          )} - ${formatter.format(el.input.maxPrice ?? 0)}`,
          Jobs: `${el.input.jobs.map((j) => j.name).join(', ')}`,
        }))
      )
    );
  }
}
