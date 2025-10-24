using DcsExamPortal.Api.DTOs;
using DcsExamPortal.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;

namespace DcsExamPortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly string _key = "rzp_test_RUZGsF4cvLu6a6";     
        private readonly string _secret = "1cogGXYb6CF7Ol9B7gaZSu24";   
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] PaymentRequestDto dto)
        {
            if (dto == null || dto.Amount <= 0)
                return BadRequest(ApiResponse.Error("Invalid payment request."));

            var result = _paymentService.CreateOrder(dto);
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPost("verify")]
        public IActionResult VerifyPayment([FromBody] PaymentVerifyDto dto)
        {
            if (dto == null)
                return BadRequest(ApiResponse.Error("Invalid payment verification data."));

            var result = _paymentService.VerifyPayment(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder([FromBody] PaymentOrderModel model)
        {
            try
            {
                RazorpayClient client = new RazorpayClient(_key, _secret);
                var options = new Dictionary<string, object>
            {
                { "amount", model.Amount * 100 }, 
                { "currency", model.Currency ?? "INR" },
                { "payment_capture", 1 }
            };
                Order order = client.Order.Create(options);

                var response = new RazorpayOrderResponse
                {
                    Key = _key, 
                    OrderId = order["id"].ToString(),
                    Amount = Convert.ToInt32(order["amount"]),
                    Currency = order["currency"].ToString()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("Save")]
        public async Task<IActionResult> SavePayment([FromBody] PaymentDto model)
        {
            var response = _paymentService.SubmitPaymentDetails(model);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


    }
    public class PaymentOrderModel
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
    }
}
