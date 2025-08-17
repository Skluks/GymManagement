using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Admins.Persistence;

public class AdminsRepository : IAdminsRepository
{
    private readonly GymDbContext _dbContext;

    public AdminsRepository(GymDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Admin?> GetByIdAsync(Guid adminId)
    {
        return _dbContext.Admins.FirstOrDefaultAsync(a => a.Id == adminId);
    }

    public Task UpdateAsync(Admin admin)
    {
        _dbContext.Admins.Update(admin);

        return Task.CompletedTask;
    }
}