
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("/api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly UserService _usersService;

    public UsersController(UserService usersService)
    {
        _usersService = usersService;
    }

    // Post: "/api/v1/users" => create new users
    [HttpPost]
    public async Task<IActionResult> CreateUsersAsync([FromBody] CreateUserDto newUser)
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
            var user = await _usersService.CreateUserAsyncService(newUser);
            return ApiResponses.Created(user, "User created successfully");
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

    // Get: "/api/v1/users" => get all the users 
    [HttpGet]
    public async Task<IActionResult> GetUsersAsync(int pageNumber = 1, int pageSize = 3, string? searchQuery = null, string? sortBy = null, string? sortOrder = "asc")
    {
        try
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return ApiResponses.BadRequest("Page number and page size should be greater than 0.");
            }
            var users = await _usersService.GetUsersAsyncService(pageNumber, pageSize, searchQuery, sortBy, sortOrder);
            if (users.Count() == 0)
            {
                return ApiResponses.NotFound("The list of users is empty or you try to search for not exisiting user name.");
            }
            return ApiResponses.Success(users, "Return the list of users successfully");
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

    // Get: "api/v1/users/{userId}" => get specific users by id 
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserByIdAsync(Guid userId)
    {
        try
        {
            var user = await _usersService.GetUserByIdAsyncService(userId);
            if (user == null)
            {
                return ApiResponses.NotFound("User does not exisit");
            }
            return ApiResponses.Success(user, "User is returned successfully");
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
    // Delet: "api/v1/users/{usersId}" => delete user by id 
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUserAsync(Guid userId)
    {
        try
        {
            var user = await _usersService.DeleteUserByIdAsyncService(userId);
            if (!user)
            {
                return ApiResponses.NotFound("User does not exisit");
            }
            return ApiResponses.Success("User deleted successfully");
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

    // Put: "api/v1/users/{userId}" => update user by id 
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdataUserByIdAsync(Guid userId, [FromBody] UpdateUserDto updateUser)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine("Validation errors:");
            errors.ForEach(error => Console.WriteLine(error));
            return ApiResponses.BadRequest("Invalid User Data");
        }
        try
        {
            var user = await _usersService.UpdateUserByIdAsyncService(userId, updateUser);
            if (user == null)
            {
                return ApiResponses.NotFound("User does not exisit");
            }
            return ApiResponses.Success("User updated successfully");
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