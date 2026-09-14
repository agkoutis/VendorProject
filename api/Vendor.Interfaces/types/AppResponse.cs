namespace Vendor.Interfaces.Types
{
    public class AppResponse<T>
    {
        public T Data { get; set; } = default!;

        public string Error { get; set; } = string.Empty;

        public bool HasError => !string.IsNullOrEmpty(Error);

        public static AppResponse<T> Success(T value)
        {
            return new AppResponse<T>
            {
                Data = value,
                Error = string.Empty
            };
        }

        public static AppResponse<T> Fail(string error)
        {
            return new AppResponse<T>
            {
                Data = default!,
                Error = error
            };
        }
    }
}
