using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    Task<UserDto> CreateUserAsyncService(CreateUserDto newUser);
    Task<List<UserDto>> GetUsersAsyncService(int pageNumber, int pageSize, string searchQuery, string sortBy, string sortOrder);
    Task<UserDto?> GetUserByIdAsyncService(Guid userId);
    Task<bool> DeleteUserByIdAsyncService(Guid userId);
    Task<UserDto?> UpdateUserByIdAsyncService(Guid userId, UpdateUserDto updateUser);
}
public class UserService : IUserService
{
    private readonly AppDBContext _appDbContext;
    private readonly IMapper _mapper;

    public UserService(AppDBContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<UserDto> CreateUserAsyncService(CreateUserDto newUser)
    {
        try
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            newUser.Password = hashedPassword;
            var user = _mapper.Map<User>(newUser);
            await _appDbContext.Users.AddAsync(user);
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

    public async Task<List<UserDto>> GetUsersAsyncService(int pageNumber, int pageSize, string searchQuery, string sortBy, string sortOrder)
    {
        try
        {
            var users = await _appDbContext.Users.ToListAsync();
            // using query to search for all the users whos matching the name otherwise return null
            var filterUsers = users.AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                filterUsers = filterUsers.Where(c => c.FirstName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
                if (filterUsers.Count() == 0)
                {
                    var exisitingUser = _mapper.Map<List<UserDto>>(filterUsers);
                    return exisitingUser;
                }
            }
            // sort the list of users dependes on first or last name as desc or asc othwewise shows the default
            filterUsers = sortBy?.ToLower() switch
            {
                "firstname" => sortOrder == "desc" ? filterUsers.OrderByDescending(c => c.FirstName) : filterUsers.OrderBy(c => c.FirstName),
                "lastname" => sortOrder == "desc" ? filterUsers.OrderByDescending(c => c.LastName) : filterUsers.OrderBy(c => c.LastName),
                _ => filterUsers.OrderBy(c => c.FirstName) // default 
            };
            // return the result in pagination 
            var paginationResult = filterUsers.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var userData = _mapper.Map<List<UserDto>>(paginationResult);
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
    public async Task<UserDto?> GetUserByIdAsyncService(Guid userId)
    {
        try
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
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

    public async Task<bool> DeleteUserByIdAsyncService(Guid userId)
    {
        try
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }
            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync();
            return true;
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

    public async Task<UserDto?> UpdateUserByIdAsyncService(Guid userId, UpdateUserDto updateUser)
    {
        try
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            user.FirstName = updateUser.FirstName ?? user.FirstName;
            user.LastName = updateUser.LastName ?? user.LastName;
            user.Email = updateUser.Email ?? user.Email;
            if (updateUser.Password != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateUser.Password);
            }
            user.Phone = updateUser.Phone ?? user.Phone;

            _appDbContext.Update(user);
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
}