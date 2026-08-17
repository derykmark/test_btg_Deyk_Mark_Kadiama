using BankKRT.Domain.Entities;
using BankKRT.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankKRT.Infrastructure.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        return await _context.Accounts.FindAsync(id);
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _context.Accounts.ToListAsync();
    }

    public async Task<Account?> GetByCpfAsync(string cpf)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a => ((string)(object)a.Cpf) == cpf);
    }

    public async Task<Account> AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task UpdateAsync(Account account)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Account account)
    {
        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Accounts.AnyAsync(a => a.Id == id);
    }
}
