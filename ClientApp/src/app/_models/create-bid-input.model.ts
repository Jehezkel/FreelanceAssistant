export interface CreateBidInput {
  project_id: number;
  amount: number;
  period: number;
  milestone_percentage: number;
  description: string;
}
