namespace DcsExamPortal.Api.DTOs
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static ApiResponse Ok(object data, string message = "Success") =>
            new ApiResponse { Success = true, Message = message, Data = data };

        public static ApiResponse Error(string message) =>
            new ApiResponse { Success = false, Message = message };
    }
}
