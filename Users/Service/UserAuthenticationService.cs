using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace UserAuthenticationWebApi2.Services
{
    public interface IUserAuthentication
    {
        public Task<UserDto> RegisterUserAsyncService(RegisterUserDto newUser);
        public Task<string?> LoginService(UserLoginDto userLoginDto);
        string GenerateJwtToken(User user);
    }
    public class UserAuthenticationService : IUserAuthentication
    {
        private readonly AppDBContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserAuthenticationService(AppDBContext appDbContext, IConfiguration configuration, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
            _mapper = mapper;
        }

        // register user
        public async Task<UserDto> RegisterUserAsyncService(RegisterUserDto newUser)
        {
            try
            {
                var user = _mapper.Map<User>(newUser);
                await _appDbContext.Users.AddAsync(user);
                await _appDbContext.SaveChangesAsync();
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                newUser.Password = hashedPassword;
                user = _mapper.Map<User>(newUser);
                _appDbContext.Users.Update(user);
                await _appDbContext.SaveChangesAsync();
                var userData = _mapper.Map<UserDto>(user);
                return userData;
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update exceptions
                Console.WriteLine($"Database Update Error: {dbEx.Message}");
                throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred. Please try again later.");
            }

        }
        // login the user 
        public async Task<string?> LoginService(UserLoginDto userLoginDto)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == userLoginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password))
            {
                return null;
            }
            var token = GenerateJwtToken(user);
            return token;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = Environment.GetEnvironmentVariable("Jwt__KEY") ?? throw new InvalidOperationException("JWT Key is missing in configuration.");
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            }),
                Expires = DateTime.UtcNow.AddHours(2),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

                Issuer = Environment.GetEnvironmentVariable("Jwt__ISSUER"),
                Audience = Environment.GetEnvironmentVariable("Jwt__AUDIENCE")
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}