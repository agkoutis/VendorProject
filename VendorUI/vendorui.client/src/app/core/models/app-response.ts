export interface AppResponse<T> {
  data: T;
  error: string | null;
  hasError: boolean;
}
