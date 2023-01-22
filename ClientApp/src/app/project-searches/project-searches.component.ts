import { Component } from '@angular/core';
import { ProjectSearchesService } from '@services/project-searches.service';

@Component({
  selector: 'app-project-searches',
  templateUrl: './project-searches.component.html',
  styleUrls: ['./project-searches.component.css'],
})
export class ProjectSearchesComponent {
  searches$ = this.searchesService.searches$;
  constructor(private searchesService: ProjectSearchesService) {}
}
