using Microsoft.EntityFrameworkCore;
using WebshopPractice.Server.Data.Context;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Data.Repositories;

public class UserRepository(WebshopDbContext dbContext)
{
    private readonly WebshopDbContext _dbContext = dbContext;

    public async Task<User> GetUser(int id)
    {
        return await _dbContext.Users.SingleAsync(u => u.Id == id);
    }

    public async Task<User> GetUser(string username)
    {
        return await _dbContext.Users.SingleAsync(u => u.Username == username);
    }
}
