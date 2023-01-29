import { Job } from './job.model';

export class ProjectSearch {
  id: number;
  userName: string;
  input: SearchInput;
}
export class SearchInput {
  minPrice?: number;
  maxPrice?: number;
  jobs: Job[];
  constructor() {}
}
