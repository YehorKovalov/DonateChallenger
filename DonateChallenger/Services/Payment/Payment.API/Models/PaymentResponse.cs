namespace Payment.API.Models
{
    public class PaymentResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public T Response { get; set; } = default!;
        public Exception? Error { get; set; }
    }
}
