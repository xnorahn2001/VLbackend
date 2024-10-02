using Microsoft.AspNetCore.Mvc;

[ApiController, Route("/api/v1/payments")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    // Post: "/api/v1/payments" => create new payment
    [HttpPost]
    public async Task<IActionResult> CreatePaymentAsync([FromBody] CreatePaymentDto newPayment)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine("Validation errors:");
            errors.ForEach(error => Console.WriteLine(error));

            // Return a custom response with validation errors
            return BadRequest(new { Message = "Validation failed", Errors = errors });
        }
        try
        {
            var payment = await _paymentService.CreatePaymentAsyncService(newPayment);
            return ApiResponses.Created(payment, "Payment created successfully");
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, "Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, "Server error: " + ex.Message);
        }
    }

    // Get: "/api/v1/payments" => get all the payments 
    [HttpGet]
    public async Task<IActionResult> GetPaymentAsync()
    {
        try
        {
            var payments = await _paymentService.GetPaymentsAsyncService();
            if (payments.Count() == 0)
            {
                return ApiResponses.NotFound("The list of payments is empty");
            }
            return ApiResponses.Success(payments, "Return the list of payments successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }

    // Get: "api/v1/payments/{paymentId}" => get specific payment by id 
    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetPaymentByIdAsync(Guid paymentId)
    {
        try
        {
            var payment = await _paymentService.GetPaymentByIdAsyncService(paymentId);
            if (payment == null)
            {
                return ApiResponses.NotFound("Payment does not exisit");
            }
            return ApiResponses.Success(payment, "Payment is returned successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }


    }
    // Delet: "api/v1/payments/{paymentId}" => delete payment by id 
    [HttpDelete("{paymentId}")]
    public async Task<IActionResult> DeletePaymentAsync(Guid paymentId)
    {
        try
        {
            var payment = await _paymentService.DeletePaymentByIdAsyncService(paymentId);
            if (!payment)
            {
                return ApiResponses.NotFound("Payment does not exisit");
            }
            return ApiResponses.Success("Payment deleted successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }

    // Put: "api/v1/payments/{paymentId}" => update payment by id 
    [HttpPut("{paymentId}")]
    public async Task<IActionResult> UpdataPaymentByIdAsync(Guid paymentId, [FromBody] UpdatePaymentDto updatePayment)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine("Validation errors:");
            errors.ForEach(error => Console.WriteLine(error));
            return ApiResponses.BadRequest("Invalid Customer Data");
        }
        try
        {
            var payment = await _paymentService.UpdatePaymentByIdAsyncService(paymentId, updatePayment);
            if (payment == null)
            {
                return ApiResponses.NotFound("Payment does not exisit");
            }
            return ApiResponses.Success("Payment updated successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }

    }

}