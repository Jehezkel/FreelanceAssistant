export interface HttpRequestState<T> {
  isLoading: boolean;
  value?: T;
  error?: Error;
}
