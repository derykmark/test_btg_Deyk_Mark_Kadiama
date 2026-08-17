using BankKRT.Domain.Entities;

namespace BankKRT.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int id);
    Task<IEnumerable<Account>> GetAllAsync();
    Task<Account?> GetByCpfAsync(string cpf);
    Task<Account> AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task DeleteAsync(Account account);
    Task<bool> ExistsAsync(int id);
}
