import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SearchInput } from 'src/app/_models/project-search';

@Component({
  selector: 'app-search-form',
  templateUrl: './search-form.component.html',
  styleUrls: ['./search-form.component.css'],
})
export class SearchFormComponent {
  @Input() searchInput: SearchInput = new SearchInput();
  @Output() result = new EventEmitter<SearchInput>();
}
