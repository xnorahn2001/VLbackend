
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("/api/customers")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    // Post: "/api/customers" => create new customer
    [HttpPost]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerDto newCustomer)
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
            var customer = await _customerService.CreateCustomerAsyncService(newCustomer);
            return ApiResponses.Created(customer, "Customer created successfully");
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

    // Get: "/api/customers" => get all the customers 
    [HttpGet]
    public async Task<IActionResult> GetCustomersAsync()
    {
        try
        {
            var customers = await _customerService.GetCustomersAsyncService();
            if (customers.Count() == 0)
            {
                return ApiResponses.NotFound("The list of customers is empty");
            }
            return ApiResponses.Success(customers, "Return the list of customers successfully");
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

    // Get: "api/customers/{customerId}" => get specific customer by id 
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCustomerByIdAsync(Guid customerId)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsyncService(customerId);
            if (customer == null)
            {
                return ApiResponses.NotFound("Customer does not exisit");
            }
            return ApiResponses.Success(customer, "Customer is returned successfully");
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
    // Delet: "api/customers/{customerId}" => delete customers by id 
    [HttpDelete("{customerId}")]
    public async Task<IActionResult> DeleteCustomerAsync(Guid customerId)
    {
        try
        {
            var customer = await _customerService.DeleteCustomerByIdAsyncService(customerId);
            if (!customer)
            {
                return ApiResponses.NotFound("Customer does not exisit");
            }
            return ApiResponses.Success("Customer deleted successfully");
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

    // Put: "api/customers/{customersId}" => update customer by id 
    [HttpPut("{customerId}")]
    public async Task<IActionResult> UpdataCustomerByIdAsync(Guid customerId, [FromBody] UpdateCustomerDto updateCustomer)
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
            var customer = await _customerService.UpdateCustomerByIdAsyncService(customerId, updateCustomer);
            if (customer == null)
            {
                return ApiResponses.NotFound("Customer does not exisit");
            }
            return ApiResponses.Success("Customer updated successfully");
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