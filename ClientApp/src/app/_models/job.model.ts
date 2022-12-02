export interface Job {
  id: number;
  name: string;
  selected: boolean;
  category: Category;
}
export class Category {
  id: number;
  name: string;
}
