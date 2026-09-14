export interface AppResponse<T> {
  data: T;
  error: string;
  hasError: boolean;
}
