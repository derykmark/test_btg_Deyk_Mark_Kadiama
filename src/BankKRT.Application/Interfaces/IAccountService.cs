using System.Collections.Generic;
using System.Threading.Tasks;
using BankKRT.Application.DTOs;

namespace BankKRT.Application.Interfaces;

public interface IAccountService
{
    Task<AccountResponse> CreateAsync(CreateAccountRequest request);
    Task<AccountResponse?> GetByIdAsync(int id);
    Task<IEnumerable<AccountResponse>> GetAllAsync();
    Task<AccountResponse> UpdateAsync(int id, UpdateAccountRequest request);
    Task DeleteAsync(int id);
}
