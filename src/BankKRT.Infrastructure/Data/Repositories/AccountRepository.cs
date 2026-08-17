using BankKRT.Domain.Entities;
using BankKRT.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankKRT.Infrastructure.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<AccountRepository> _logger;

    public AccountRepository(AppDbContext context, ILogger<AccountRepository> logger)
    {
        _context = context;
        _logger = logger;
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
            var result = await _context.Accounts
            .FirstOrDefaultAsync(a => ((string)(object)a.Cpf) == cpf);

            if (result != null)
            {
                _logger.LogInformation("Found account {AccountId} for CPF {Cpf}", result.Id, BankKRT.Shared.Logging.CpfMasking.Mask((string)result.Cpf));
            }

            return result;
        }

    public async Task<Account> AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
            _logger.LogInformation("Added account {AccountId} for CPF {Cpf}", account.Id, BankKRT.Shared.Logging.CpfMasking.Mask((string)account.Cpf));
            return account;
        }

    public async Task UpdateAsync(Account account)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
            _logger.LogInformation("Updated account {AccountId} for CPF {Cpf}", account.Id, BankKRT.Shared.Logging.CpfMasking.Mask((string)account.Cpf));
        }

    public async Task DeleteAsync(Account account)
    {
        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted account {AccountId} for CPF {Cpf}", account.Id, BankKRT.Shared.Logging.CpfMasking.Mask((string)account.Cpf));
        }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Accounts.AnyAsync(a => a.Id == id);
    }
}
