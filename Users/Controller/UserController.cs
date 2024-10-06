using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("/api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _usersService;

    public UsersController(IUserService usersService)
    {
        _usersService = usersService;
    }

    // Get: "/api/v1/users" => get all the users 
    [Authorize (Roles = "Admin")]
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
    [Authorize]
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
    [Authorize]
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
    [Authorize]
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