namespace Payment.API.Models
{
    public class PaymentResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Response { get; set; }
        public Exception Error { get; set; }
    }
}
