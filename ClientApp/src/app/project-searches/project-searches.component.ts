import { Component, OnInit } from '@angular/core';
import { ProjectSearchesService } from '@services/project-searches.service';

@Component({
  selector: 'app-project-searches',
  templateUrl: './project-searches.component.html',
  styleUrls: ['./project-searches.component.css'],
})
export class ProjectSearchesComponent implements OnInit {
  searches$ = this.searchesService.searches$;
  constructor(private searchesService: ProjectSearchesService) {}
  ngOnInit(): void {
    this.searchesService.refreshSearches().subscribe();
  }
}
