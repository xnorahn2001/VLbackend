// Authentication controller that deals with registeration and log in 
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationWebApi2.Services;

[ApiController, Route("/api/v1/users")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserAuthentication _usersAuthService;

    public AuthenticationController(IUserAuthentication userService)
    {
        _usersAuthService = userService;
    }

    // Post: "/api/v1/users/register" => register for new users
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUsersAsync([FromBody] RegisterUserDto newUser)
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
            var user = await _usersAuthService.RegisterUserAsyncService(newUser);
            return ApiResponses.Created(user, "User registered successfully");
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

    // POST: api/users/login
    [HttpPost("login")]
    public async Task<IActionResult>Login(UserLoginDto userLogin)
    {
        var token = await _usersAuthService.LoginService(userLogin);
        if (token == null){
            return ApiResponses.BadRequest("Email/Password incorrect.");
        }
        Console.WriteLine($"{token}");
        
        return ApiResponses.Success("Login successfully");
    }

}
